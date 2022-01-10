using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;

namespace ConsoleMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //连接数据库
            var client = new MongoClient("mongodb://120.78.202.190:27017,120.78.202.190:27117");
            //获取database
            var mydb = client.GetDatabase("tttt");

            #region MyRegion
            //获取collection
            var collection = mydb.GetCollection<BsonDocument>("userinfos");

            //var collection = mydb.GetCollection<Userinfo>("userinfos");
            //var filter = Builders<Userinfo>.Filter;
            //var sort = Builders<Userinfo>.Sort;
            //List<Userinfo> userinfos = collection.Find(filter.Lt("age", 25))    //查询年龄小于25岁的记录
            //                                     .Sort(sort.Descending("age"))  //按年龄进行倒序
            //                             .ToList();
            ////遍历结果
            //userinfos.ForEach(u =>
            //{
            //    Console.WriteLine($"编号：{u.userId},姓名：{u.name},年龄：{u.age},英文名：{u.ename?.ming} {u.ename?.xing},性别：{u.gender}");
            //    Console.WriteLine($"其他属性：{u.otherprops}");
            //    Console.WriteLine();
            //});

            //Console.ReadKey(); 
            #endregion

            #region MyRegion
            //待添加的document
            var doc = new BsonDocument{
                //{ "_id",7 },
                 { "Date", DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc) },
                { "name", "测试2多大sdsssds" },
                { "age", 29999 },
                 { "level", 29 },
                { "ename", new BsonDocument
                    {
                        { "firstname", "jiu" },
                        { "lastname", "wu" }
                    }
                }
            };


            collection.InsertOne(doc);

            //var Date = DateTime.Now.ToUniversalTime().Date;
            //var project = Builders<BsonDocument>.Projection;
            //var filter = Builders<BsonDocument>.Filter;
            //var docs = collection.Find(filter.Eq("Date", Date) & filter.Eq("age", 299991)).Project(project.Include("name")
            //                                     .Include("age")//包含age
            //                                     .Exclude("_id")//不包含_id
            //                           ).ToList();
            //docs.ForEach(d => Console.WriteLine(d));
            //Fileter用于过滤，如查询name = 吴九的第一条记录
            //var filter = Builders<BsonDocument>.Filter;
            ////Find(filter)进行查询
            //var doc = collection.Find(filter.Eq("name", "吴九")).FirstOrDefault();
            //Console.WriteLine(doc);

            //查询25<age<28的记录
            //var filter = Builders<BsonDocument>.Filter;
            //var docs = collection.Find(filter.Gt("age", 25) & filter.Lt("age", 28)).ToList();
            //docs.ForEach(d => Console.WriteLine(d));

            //查询年龄小于25或年龄大于28的记录
            //var filter = Builders<BsonDocument>.Filter;
            //var docs = collection.Find(filter.Eq("age", 29999)).ToList();
            //docs.ForEach(d => Console.WriteLine(d));

            //查询存在address字段的记录
            //var filter = Builders<BsonDocument>.Filter;
            //var docs = collection.Find(filter.Exists("address")).ToList();
            //docs.ForEach(d => Console.WriteLine(d));

            //查询age<26的记录，按年龄倒序排列
            //var filter = Builders<BsonDocument>.Filter;
            //var sort = Builders<BsonDocument>.Sort;
            //var docs = collection.Find(filter.Lt("age", 26))//过滤
            //                     .Sort(sort.Descending("age")).ToList();//按age倒序
            //docs.ForEach(d => Console.WriteLine(d));

            //查询age<26的记录 包含name age 排除 _id
            //var project = Builders<BsonDocument>.Projection;
            //var filter = Builders<BsonDocument>.Filter;
            //var docs = collection.Find(filter.Lt("age", 26))//过滤
            //                     .Project(project.Include("name")//包含name
            //                                     .Include("age")//包含age
            //                                     .Exclude("_id")//不包含_id
            //                           ).ToList();
            //docs.ForEach(d => Console.WriteLine(d));

            //var filter = Builders<BsonDocument>.Filter;
            //var update = Builders<BsonDocument>.Update;
            //var project = Builders<BsonDocument>.Projection;
            //将张三的年龄改成18
            //collection.UpdateOne(filter.Eq("name", "吴九"), update.Set("age", 11));
            ////查询张三的记录
            //var doc = collection.Find(filter.Eq("name", "吴九"))
            //                    .Project(project.Include("age").Include("name"))
            //                    .FirstOrDefault();
            //Console.WriteLine(doc);

            //var filter = Builders<BsonDocument>.Filter;
            //var update = Builders<BsonDocument>.Update;
            //var options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>() { IsUpsert = true };
            //var project = Builders<BsonDocument>.Projection;

            ////将张三的年龄改成18
            //collection.FindOneAndUpdate(filter.Eq("age", 9999)&filter.Eq("number", 9999), update.Set("name", "修改了111222").Set("yyy", "修改了1111222"), options);
            ////查询张三的记录
            //var doc = collection.Find(filter.Eq("age", 9999)& filter.Eq("number", 9999))
            //                    .Project(project.Include("age").Include("name"))
            //                    .ToList();
            //doc.ForEach(d => Console.WriteLine(d));

            //var filter = Builders<BsonDocument>.Filter;
            //var project = Builders<BsonDocument>.Projection;
            ////删除名字为张三的记录
            //collection.DeleteOne(filter.Eq("name", "吴九"));
            //var docs = collection.Find(filter.Empty)
            //                .Project(project.Include("age").Include("name").Include("mark"))
            //                .ToList();
            //docs.ForEach(d => Console.WriteLine(d));

            //var filter = Builders<BsonDocument>.Filter;
            //var project = Builders<BsonDocument>.Projection;
            ////删除age>25的记录
            //DeleteResult result = collection.DeleteMany(filter.Gt("age", 25));
            //Console.WriteLine($"一共删除了{result.DeletedCount}条记录");
            //var docs = collection.Find(filter.Empty)
            //                .Project(project.Include("age").Include("name").Include("mark"))
            //                .ToList();
            //docs.ForEach(d => Console.WriteLine(d)); 
            #endregion

            #region MyRegion
            ////获取collection
            //var stuCollection = mydb.GetCollection<Student>("students");
            //var clsCollection = mydb.GetCollection<Classx>("classes");
            ////查找年龄大于22的学生
            //Console.WriteLine("-------------查找年龄大于22的学生列表--------------");

            ////2.点语法
            //List<Student> stuList2 = stuCollection.AsQueryable().Where(s => s.age > 22).ToList();
            //stuList2.ForEach(stu => Console.WriteLine($"姓名:{stu?.stuName},  年龄:{stu?.age}"));
            //Console.WriteLine();

            ////表连接查询，查询各个学生的班级名
            //Console.WriteLine("-------------表连接，查询学生的班级名----------------");
            ////1.query语法
            //var result1 = from stu in stuCollection.AsQueryable()
            //              join cls in clsCollection.AsQueryable()
            //                on stu.classNo equals cls.no
            //              select new { stuno = stu.no, stu.stuName, cls.clsName };
            ////2.点语法
            //var result2 = stuCollection.AsQueryable().Join(
            //                clsCollection.AsQueryable(),
            //                stu => stu.classNo,
            //                cls => cls.no,
            //                (stu, cls) => new { stuno = stu.no, stu.stuName, cls.clsName }
            //             );
            ////遍历结果
            //foreach (var item in result2)
            //{
            //    Console.WriteLine($"学号:{item.stuno}, 姓名:{item.stuName}, 班级:{item.clsName}");
            //}

            //Console.ReadKey(); 
            #endregion

            //var collection = mydb.GetCollection<BsonDocument>("AdvertiseFee");

            Console.ReadKey();
        }
    }
    /// <summary>
    /// 学生类
    /// </summary
    public class Student
    {
        public int no { get; set; }//学号
        public string stuName { get; set; }//姓名
        public int age { get; set; }//年龄
        public int classNo { get; set; }//班级编号
        [BsonExtraElements]
        public BsonDocument others { get; set; }
    }
    /// <summary>
    /// 班级类
    /// </summary>
    public class Classx
    {
        public int no { get; set; }//班级编号
        public string clsName { get; set; }//班级名
        [BsonExtraElements]
        public BsonDocument others { get; set; }
    }
    /// <summary>
    /// 用户类
    /// </summary>
    public class Userinfo
    {
        [BsonId]
        public int userId { get; set; }//id
        public string name { get; set; }//姓名
        public int age { get; set; }//年龄
        public Ename ename { get; set; }//英文名
        [BsonDefaultValue('男')]
        public char gender { get; set; }
        [BsonIgnore]
        public string nickname { get; set; }//昵称
        [BsonExtraElements]
        public BsonDocument otherprops { get; set; }//其他属性
    }
    /// <summary>
    /// 英文名
    /// </summary>
    public class Ename
    {
        [BsonElement("firstname")]
        public string ming { get; set; }
        [BsonElement("lastname")]
        public string xing { get; set; }
    }
}
