using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp.TaskManagers
{
    public class MessageQueueManager<T>
    {
        private readonly BlockingCollection<T> _messageCollection = new BlockingCollection<T>(10000);


        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;


        public void Enqueue(T message)
        {
            if (_messageCollection.Count < _messageCollection.BoundedCapacity)
                _messageCollection.Add(message);
            if (_cancellationToken.IsCancellationRequested && !_messageCollection.IsCompleted)
                _messageCollection.CompleteAdding();
        }


        public void Dequeue(Action<T> writeAction)
        {
            foreach (var message in _messageCollection.GetConsumingEnumerable())
            {
                writeAction(message);
                Thread.Sleep(10);
            }
        }


        public void Setup()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }


        public void Cancel()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel(false);
            if (!_messageCollection.IsCompleted) _messageCollection.CompleteAdding();
        }
        public int GetCount()
        {
                return  _messageCollection.Count;
        }

        public bool IsCanEnqueue
        {
            get
            {
                return _messageCollection.BoundedCapacity > _messageCollection.Count;
            }
        }
    }
}
