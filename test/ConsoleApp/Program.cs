using ConsoleApp.TaskManagers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static int sum = 0;
        private static Semaphore _pool;
        // 判断十个线程是否结束了。
        private static int isComplete = 0;

        // 控制第一个线程
        // 第一个线程开始时，AutoResetEvent 处于终止状态，无需等待信号
        private static AutoResetEvent oneResetEvent = new AutoResetEvent(true);

        // 控制第二个线程
        // 第二个线程开始时，AutoResetEvent 处于非终止状态，需要等待信号
        private static AutoResetEvent twoResetEvent = new AutoResetEvent(false);

        // 线程通知
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);

        private readonly static MessageQueueManager<string> queue = new MessageQueueManager<string>();
        //private static EventWaitHandle _wh = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int a = 0;
            //DealData();
            Consumer();
            while (a<4)
            {
                a++;
                Run();
                Thread.Sleep(10000);
            }

            Console.WriteLine("主线程继续执行");
           


            #region Interlocked 
            //AddOne();
            //AddOne();
            //AddOne();
            //AddOne();
            //AddOne();
            //Console.WriteLine("sum = " + sum);


            //int location1 = 1;
            //int value = 2;
            //int comparand = 3;

            //Console.WriteLine("运行前：");
            //Console.WriteLine($" location1 = {location1}    |   value = {value} |   comparand = {comparand}");

            //Console.WriteLine("当 location1 != comparand 时");
            //int result = Interlocked.CompareExchange(ref location1, value, comparand);
            //Console.WriteLine($" location1 = {location1} | value = {value} |  comparand = {comparand} |  location1 改变前的值  {result}");

            //Console.WriteLine("当 location1 == comparand 时");
            //comparand = 1;
            //result = Interlocked.CompareExchange(ref location1, value, comparand);
            //Console.WriteLine($" location1 = {location1} | value = {value} |  comparand = {comparand} |  location1 改变前的值  {result}");



            //int a = 1;
            //int b = 5;

            //// a 改变前为1
            //int result1 = Interlocked.Exchange(ref a, 2);

            //Console.WriteLine($"a新的值 a = {a}   |  a改变前的值 result1 = {result1}");

            //Console.WriteLine();

            //// a 改变前为 2，b 为 5
            //int result2 = Interlocked.Exchange(ref a, b);

            //Console.WriteLine($"a新的值 a = {a}   | b不会变化的  b = {b}   |   a 之前的值  result2 = {result2}");


            //for (int i = 0; i < 5; i++)
            //{
            //    Thread thread = new Thread(AddOne);
            //    thread.Start();
            //}

            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //Console.WriteLine("sum = " + sum);
            #endregion

            #region AutoResetEvent

            new Thread(DoOne).Start();
            new Thread(DoTwo).Start();

            Console.ReadKey();


            // 创建线程
            new Thread(DoOne).Start();

            // 用于不断向另一个线程发送信号
            while (true)
            {
                Console.ReadKey();
                resetEvent.Set();           // 发生通知，设置终止状态
            }
            #endregion

            #region Semaphore
            //_pool = new Semaphore(0, 5);
            //_pool.Release(5);
            //new Thread(AddOne).Start();
            //Thread.Sleep(TimeSpan.FromSeconds(10));
            //_pool.Close();


            //_pool = new Semaphore(0, 3);
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(AddOne));
            //    thread.Start(i + 1);
            //}
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("任意按下键(不要按关机键)，可以打开资源池");
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.ReadKey();

            //// 准许三个线程进入
            //_pool.Release(3);

            //// 这里没有任何意义，就单纯为了演示查看结果。
            //// 等待所有线程完成任务
            //while (true)
            //{
            //    if (isComplete >= 10)
            //        break;
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //}
            //Console.WriteLine("sum = " + sum);

            //// 释放池
            //_pool.Close(); 
            #endregion

            #region MyRegion
            //Thread thread1 = new Thread(Test1);
            //Thread thread2 = new Thread(Test2);

            //thread1.Start();
            //thread2.Start();

            //Console.ReadKey();
            //Thread thread = new Thread(OneTest);
            //thread.Name = "Test";
            //thread.Start();
            //Console.ReadKey();
            //Task<int> taskB = new Task<int>(() =>
            //{
            //    Console.WriteLine("我是前驱任务");
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //    return 666;
            //});

            //ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter awaiterB = taskB.ConfigureAwait(false).GetAwaiter();

            //awaiterB.OnCompleted(() =>
            //{
            //    Console.WriteLine("前驱任务完成时，我就会继续执行");
            //});
            //taskB.Start();

            //Console.ReadKey();


            //Task<int> taskA = new Task<int>(() =>
            //{
            //    Console.WriteLine("我是前驱任务");
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //    return 666;
            //});

            //TaskAwaiter<int> awaiter = taskA.GetAwaiter();

            //awaiter.OnCompleted(() =>
            //{
            //    Console.WriteLine("前驱任务完成时，我就会继续执行");
            //});
            //taskA.Start();

            //Console.ReadKey();

            //// 实例化任务类
            //MyTaskClass myTask = new MyTaskClass();

            //for (int i = 0; i < 10; i++)
            //{
            //    int tmp = i;
            //    myTask.AddTask(() =>
            //    {
            //        Console.WriteLine("     任务 1 Start");
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        Console.WriteLine("     任务 1 End");
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //    });
            //}

            //// 相当于 Task.WhenAll()
            //Task task = myTask.StartAsync();
            //Thread.Sleep(TimeSpan.FromSeconds(1));
            //Console.WriteLine($"任务是否被取消：{task.IsCanceled}");

            //// 取消任务
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("按下任意键可以取消任务");
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.ReadKey();

            //var t = myTask.Cancel();    // 取消任务
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //Console.WriteLine($"任务是否被取消：【{task.IsCanceled}】");

            //Console.ReadKey();

            //Console.WriteLine("开始执行...");

            //Thread t = new Thread(PrintNumbers);
            //Thread t2 = new Thread(DoNothing);

            //// 使用ThreadState查看线程状态 此时线程未启动，应为Unstarted
            //Console.WriteLine($"Check 1 :{t.ThreadState}");

            //t2.Start();
            //t.Start();

            //// 线程启动， 状态应为 Running
            //Console.WriteLine($"Check 2 :{t.ThreadState}");

            //// 由于PrintNumbers方法开始执行，状态为Running
            //// 但是经接着会执行Thread.Sleep方法 状态会转为 WaitSleepJoin
            //for (int i = 1; i < 30; i++)
            //{
            //    Console.WriteLine($"Check 3 : {t.ThreadState}");
            //}

            //// 延时一段时间，方便查看状态
            //Thread.Sleep(TimeSpan.FromSeconds(6));

            //// 终止线程
            //t.Abort();

            //Console.WriteLine("t线程被终止");

            //// 由于该线程是被Abort方法终止 所以状态为 Aborted或AbortRequested
            //Console.WriteLine($"Check 4 : {t.ThreadState}");
            //// 该线程正常执行结束 所以状态为Stopped
            //Console.WriteLine($"Check 5 : {t2.ThreadState}");

            //Console.ReadKey();

            //Parallel.For(0, 10, i =>
            //{
            //    Console.WriteLine("idx:{0}, task:{1}, thread:{2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            //});//从结果可以看出，顺序是不能保证的。 
            #endregion
        }


        public static void Run()
        {
            Producer();
           
        }

        public static void DealData()
        {
            int pageSize = 200;

            //创建队列
            var queue = new MessageQueueManager<string>();

            queue.Setup();
            //开启生产者线程
            var producerManager = new TaskManager();
            producerManager.Setup(5);
            producerManager.Start((index) =>
            {
                var pageIndex = Convert.ToInt32(index) + 1;
                CountdownEvent countd = new CountdownEvent(pageSize);
                while (true)
                {
                    try
                    {
                        var modelList = GetModelList(pageIndex, pageSize);

                        if (modelList == null)
                        {
                            break;
                        }

                        //循环学习统计加入消费者队列
                        foreach (var model in modelList)
                        {
                            ////自旋
                            while (!queue.IsCanEnqueue)
                            {
                                Thread.Sleep(5 * 1000);
                            }
                            Console.WriteLine(model + $"入队,当前的线程是{Thread.CurrentThread.ManagedThreadId}");
                            queue.Enqueue(model);
                            countd.Signal();
                        }
                        // 等他们都完成事情
                        countd.Wait();
                        Console.WriteLine($"队列中的总数是:{queue.GetCount()}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("线程任务完成");
                Console.ForegroundColor = ConsoleColor.White;
            });
            Thread.Sleep(3000);
            //消费者线程
            var consumerManager = new TaskManager();
            //这里设置一个线程进行处理数据
            consumerManager.Setup(5);
            consumerManager.Start(() =>
            {
                if (queue.GetCount() > 0)
                {
                    //从队列中取出数据进行处理
                    queue.Dequeue((info) =>
                    {
                        try
                        {
                            //处理数据
                            Console.WriteLine(info + "队列出队" + $"当前的线程是{Thread.CurrentThread.ManagedThreadId}");
                            //Thread.Sleep(1500);
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                }
                Console.WriteLine("消费者线程完成任务");
            });

            producerManager.Wait();
            Console.WriteLine("生产者完成任务");

            queue.Cancel();
            consumerManager.Wait();
            Console.WriteLine("消费者完成任务");
            Console.WriteLine($"队列中的总数是:{queue.GetCount()}");

        }

        public static void Producer()
        {
            Thread.Sleep(2000);
            int pageSize = 5;
            //创建队列
            queue.Setup();
            //开启生产者线程
            var producerManager = new TaskManager();
            producerManager.Setup(5);
            producerManager.Start((index) =>
            {
                var pageIndex = Convert.ToInt32(index) + 1;

                while (true)
                {
                    try
                    {
                        var modelList = GetModelList(pageIndex, pageSize);
                        CountdownEvent countd = new CountdownEvent(pageSize);
                        if (modelList == null)
                        {
                            break;
                        }
                        //循环学习统计加入消费者队列
                        foreach (var model in modelList)
                        {
                            //自旋
                            while (!queue.IsCanEnqueue)
                            {
                                Thread.Sleep(5 * 1000);
                            }
                            Console.WriteLine(model + $"入队,当前的线程是{Thread.CurrentThread.ManagedThreadId}");
                            queue.Enqueue(model);
                            //Thread.Sleep(4000);
                            countd.Signal();
                        }
                        countd.Wait();

                        Console.WriteLine($"线程完成时队列中的总数是:{queue.GetCount()}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            });
            producerManager.Wait();
            queue.Cancel();
            Console.WriteLine("生产者完成任务");
            Thread.Sleep(4000);
            Consumer();
            //_wh.Set();
        }

        public static bool Consumer()
        {
            bool flag = false;

            var task = Task.Run(()=> {
                Console.WriteLine("队列等待");
                //_wh.WaitOne();
                Console.WriteLine("消费者开始任务");
                var consumerManager = new TaskManager();
                consumerManager.Setup(5);
                consumerManager.Start(() =>
                {
                    if (queue.GetCount() > 0)
                    {
                        //从队列中取出数据进行处理
                        queue.Dequeue((info) =>
                        {
                            try
                            {
                                //处理数据
                                Console.WriteLine(info + "队列出队" + $"当前的线程是{Thread.CurrentThread.ManagedThreadId}");

                            }
                            catch (Exception ex)
                            {

                            }
                        });
                    }
                    Console.WriteLine("消费者线程完成任务");
                });
                Console.WriteLine($"线程完成时队列中的总数是:{queue.GetCount()}");
               
                consumerManager.Wait();
                Console.WriteLine("消费者完成任务");
                flag = true;
            });
            //task.Wait();
            //Console.WriteLine("完成任务");
            return flag;
        }

        private static List<string> GetModelList(int pageIndex, int pageSize)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize + 1;
            List<string> list = new List<string>();
            for (int i = start; i < end; i++)
            {
                list.Add(i.ToString());
            }
            return list;
        }

        public static void AddOne()
        {
            for (int i = 0; i < 100_0000; i++)
            {

                Interlocked.Increment(ref sum);
                //sum += 1;
            }
        }
        private static void DoTwo(object obj)
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                // 等待 DoOne() 给我信号
                twoResetEvent.WaitOne();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n     DoTwo() 执行");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("\n② 按一下键，我就让DoOne运行");
                Console.ReadKey();
                oneResetEvent.Set();
                twoResetEvent.Reset();
            }
        }

        private static void DoOne(object obj)
        {
            while (true)
            {
                Console.WriteLine("\n① 按一下键，我就让DoTwo运行");
                Console.ReadKey();
                twoResetEvent.Set();
                oneResetEvent.Reset();
                // 等待 DoTwo() 给我信号
                oneResetEvent.WaitOne();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n     DoOne() 执行");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        //private static void DoOne(object obj)
        //{
        //    Console.WriteLine("等待中，请发出信号允许我运行");

        //    // 等待其它线程发送信号
        //    resetEvent.WaitOne();

        //    Console.WriteLine("\n     收到信号，继续执行");
        //    for (int i = 0; i < 5; i++) Thread.Sleep(TimeSpan.FromSeconds(0.5));

        //    resetEvent.Reset(); // 重置为非终止状态
        //    Console.WriteLine("\n第一阶段运行完毕，请继续给予指示");

        //    // 等待其它线程发送信号
        //    resetEvent.WaitOne();
        //    Console.WriteLine("\n     收到信号，继续执行");
        //    for (int i = 0; i < 5; i++) Thread.Sleep(TimeSpan.FromSeconds(0.5));

        //    Console.WriteLine("\n第二阶段运行完毕，线程结束，请手动关闭窗口");
        //}

        //public static void AddOne()
        //{
        //    _pool.WaitOne();
        //    Thread.Sleep(1000);
        //    int count = _pool.Release();
        //    Console.WriteLine("在此线程退出资源池前，资源池还有多少线程可以进入？" + count);
        //}
        //private static void AddOne(object n)
        //{
        //    Console.WriteLine($"    线程{(int)n}启动，进入队列");
        //    // 进入队列等待
        //    _pool.WaitOne();
        //    Console.WriteLine($"第{(int)n}个线程进入资源池");
        //    // 进入资源池
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Interlocked.Add(ref sum, 1);
        //        Thread.Sleep(TimeSpan.FromMilliseconds(500));
        //    }
        //    // 解除占用的资源池
        //    _pool.Release();
        //    isComplete += 1;
        //    Console.WriteLine($" 第{(int)n}个线程退出资源池");
        //}

        public static void Test1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Test1:" + i);
            }
        }
        public static void Test2()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Test2:" + i);
            }
        }
        static void DoNothing()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
        static void PrintNumbers()
        {
            Console.WriteLine("t线程开始执行...");

            // 在线程内部，可通过Thread.CurrentThread拿到当前线程Thread对象
            Console.WriteLine($"Check 6 : {Thread.CurrentThread.ThreadState}");
            for (int i = 1; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine($"t线程输出 ：{i}");
            }
        }

        public static void OneTest()
        {
            Thread thisTHread = Thread.CurrentThread;
            Console.WriteLine("线程标识：" + thisTHread.Name);
            Console.WriteLine("当前地域：" + thisTHread.CurrentCulture.Name);  // 当前地域
            Console.WriteLine("线程执行状态：" + thisTHread.IsAlive);
            Console.WriteLine("是否为后台线程：" + thisTHread.IsBackground);
            Console.WriteLine("是否为线程池线程" + thisTHread.IsThreadPoolThread);
        }
    }
}
