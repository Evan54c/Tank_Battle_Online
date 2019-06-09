using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using MyNHibernate.Model;
using MyNHibernate.Manager;

namespace MyNHibernate
{
    class Program
    {
        static void Main(string[] args)
        {
            //增
            //User user = new User() { Username = "dema" };
            //IUserManager userManager = new UserManager();
            //userManager.Add(user);

            //改 ，必须提供id（主键）
            //User user = new User() {Id = 8 ,Username = "demawq"};
            //IUserManager userManager = new UserManager();
            //userManager.Update(user);

            //删 ，只用提供id（主键）
            //User user = new User() {Id = 3};
            //IUserManager userManager = new UserManager();
            //userManager.Remove(user);

            //查 ， 通过主键查
            //IUserManager userManager = new UserManager();
            //User user = userManager.GetById(4);
            //Console.WriteLine(user.Username);
            //Console.WriteLine(user.Password);

            //查 ， 通过表项查
            //IUserManager userManager = new UserManager();
            //User user = userManager.GetByUserName("sao");
            //Console.WriteLine(user.Username);
            //Console.WriteLine(user.Password);

            //集合查询
            //IUserManager userManager = new UserManager();
            //ICollection<User> users = userManager.GetAllUsers();
            //foreach(User user in users)
            //{
            //Console.WriteLine(user.Username);
            //Console.WriteLine(user.Password);
            //}

            //验证账号密码
            //IUserManager userManager = new UserManager();
            //Console.WriteLine(userManager.VerifyUser("Evan","12345"));
            //Console.WriteLine(userManager.VerifyUser("Evans", "12345"));




            //var configuration = new Configuration();
            //configuration.Configure();//解析nhibernate.cfg.xml
            //configuration.AddAssembly("MyNHibernate"); //解析映射文件 User.hbm.xml....

            //ISessionFactory sessionFactory = null;
            //ISession session = null;
            //ITransaction transaction = null;

            //try
            //{
            //    sessionFactory = configuration.BuildSessionFactory();

            //    session = sessionFactory.OpenSession(); //打开一个跟数据库的会话

            //    //User user = new User() { Username = "mn", Password = "123a" };

            //    //session.Save(user);

            //    //事务（整体性的操作）
            //    transaction = session.BeginTransaction();
            //    //进行操作
            //    User user1 = new User() { Username = "sawo", Password = "123fea" };
            //    User user2 = new User() { Username = "dif", Password = "1qq23a" };
            //    session.Save(user1);
            //    session.Save(user2);
            //    //事务提交
            //    transaction.Commit();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}finally
            //{
            //    if (transaction != null)
            //    {
            //        transaction.Dispose();
            //    }
            //    if (session != null)
            //    {
            //        session.Close();
            //    }
            //    if (sessionFactory != null)
            //    {
            //        sessionFactory.Close();
            //    }

            //}


            Console.ReadKey();
        }
    }
}
