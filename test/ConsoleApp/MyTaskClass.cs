using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// 能够完成多个任务的异步类型
    /// </summary>
    public class MyTaskClass
    {
        private List<Action> _actions = new List<Action>();
        private CancellationTokenSource _source = new CancellationTokenSource();
        private CancellationTokenSource _sourceBak = new CancellationTokenSource();
        private Task _task;

        /// <summary>
        ///  添加一个任务
        /// </summary>
        /// <param name="action"></param>
        public void AddTask(Action action)
        {
            _actions.Add(action);
        }

        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            // _ = new Task() 对本示例无效
            _task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < _actions.Count; i++)
                {
                    int tmp = i;
                    Console.WriteLine($"第 {tmp} 个任务");
                    if (_source.Token.IsCancellationRequested)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("任务已经被取消");
                        Console.ForegroundColor = ConsoleColor.White;
                        _sourceBak.Cancel();
                        _sourceBak.Token.ThrowIfCancellationRequested();
                    }
                    _actions[tmp].Invoke();
                }
            }, _sourceBak.Token);
            return _task;
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        public Task Cancel()
        {
            _source.Cancel();

            // 这里可以省去
            _task = Task.FromCanceled<object>(_source.Token);
            return _task;
        }
    }
}
