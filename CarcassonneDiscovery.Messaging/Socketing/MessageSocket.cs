namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Xml.Serialization;

    /// <summary>
    /// Socket on client or server side.
    /// </summary>
    /// <typeparam name="S">Type of messages which are sent by this socket.</typeparam>
    /// <typeparam name="R">Type of messages which are received by this socket.</typeparam>
    public abstract class MessageSocket<S, R>
    {
        #region Static fields and constructor: Message serializers
        /// <summary>
        /// Static constructor.
        /// </summary>
        static MessageSocket()
        {
            SendMsgSerializer = new XmlSerializer(typeof(S));
            ReceiveMsgSerializer = new XmlSerializer(typeof(R));
        }

        /// <summary>
        /// Serializer of request objects.
        /// </summary>
        protected static XmlSerializer SendMsgSerializer;

        /// <summary>
        /// Serializer of response objects.
        /// </summary>
        protected static XmlSerializer ReceiveMsgSerializer;
        #endregion

        /// <summary>
        /// Event when new message was received.
        /// </summary>
        public event Action MessageReceived = () => { };

        /// <summary>
        /// Queue of unhandled responses from server.
        /// </summary>
        protected Queue<R> ReceivedQueue;

        /// <summary>
        /// Thread that listens to server responses.
        /// </summary>
        protected Thread ListeningLoopThread;

        /// <summary>
        /// Actual socket.
        /// </summary>
        protected Socket Socket;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessageSocket()
        {
            ReceivedQueue = new Queue<R>();
        }

        /// <summary>
        /// Starts connection and listening to the server.
        /// </summary>
        public void Start()
        {
            // Start the actual socket
            StartSocket();

            // Start the thread
            ListeningLoopThread = new Thread(new ThreadStart(ListeningLoop));
            ListeningLoopThread.Start();
        }

        /// <summary>
        /// Stops connection to the server.
        /// </summary>
        public void Stop()
        {
            // Stop the actual socket
            StopSocket();

            // Empty the queue
            ReceivedQueue.Clear();

            // Stop the thread
            if (ListeningLoopThread != null)
            {
                ListeningLoopThread.Abort();
            }
        }

        /// <summary>
        /// Starts the actual socket.
        /// </summary>
        protected abstract void StartSocket();

        /// <summary>
        /// Stops the actual socket.
        /// </summary>
        protected abstract void StopSocket();

        /// <summary>
        /// Loop that serves as listener to the server responses.
        /// </summary>
        protected abstract void ListeningLoop();

        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="msg">Request object.</param>
        public void SendMessage(S msg)
        {
            if (Socket != null)
            {
                var msgWriter = new StringWriter();

                SendMsgSerializer.Serialize(msgWriter, msg);
                msgWriter.Write('$');

                try
                {
                    Socket.Send(Encoding.UTF8.GetBytes(msgWriter.ToString()));
                }
                catch
                {
                    // TODO: Catch
                }
            }
        }

        /// <summary>
        /// Returns next unhandled message.
        /// </summary>
        /// <returns>Reference to unhandled message or null if there is no message.</returns>
        public R GetNextMessage()
        {
            R result = default;

            lock (ReceivedQueue)
            {
                if (ReceivedQueue.Count > 0)
                {
                    result = ReceivedQueue.Dequeue();
                }
            }

            return result;
        }

        /// <summary>
        /// Invokes the MessageReceived event.
        /// </summary>
        protected void MessageReceivedInvoke()
        {
            MessageReceived.Invoke();
        }

        /// <summary>
        /// Adds message into the message queue.
        /// </summary>
        /// <param name="msg">Received message object.</param>
        protected void EnqueueMessage(R msg)
        {
            if (msg != null)
            {
                lock (ReceivedQueue)
                {
                    ReceivedQueue.Enqueue(msg);
                }
            }
        }
    }
}