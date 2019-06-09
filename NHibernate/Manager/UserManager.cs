using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNHibernate.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MyNHibernate.Manager
{
    class UserManager : IUserManager
    {
        public void Add(User user)
        {
            //ISession session = MyNHibernateHelper.OpenSession();
            //session.Save(user);
            //session.Close();
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                using (ITransaction transcation = session.BeginTransaction()) //建立事务
                {
                    session.Save(user);   //对数据库进行操作
                    transcation.Commit(); //提交事务
                }
            }

        }

        public ICollection<User> GetAllUsers()
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();
                return users;
            }
        }

        public User GetById(int id)
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                using (ITransaction transcation = session.BeginTransaction()) //建立事务
                {
                    User user = session.Get<User>(id);   //对数据库进行操作
                    transcation.Commit(); //提交事务
                    return user;
                }
            }
        }

        public User GetByUserName(string username)
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                //ICriteria criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Eq("Username", username));
                //User user = criteria.UniqueResult<User>();
                User user = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", username)).UniqueResult<User>();
                return user;
            }
        }

        public void Remove(User user)
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                using (ITransaction transcation = session.BeginTransaction()) //建立事务
                {
                    session.Delete(user);   //对数据库进行操作
                    transcation.Commit(); //提交事务
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                using (ITransaction transcation = session.BeginTransaction()) //建立事务
                {
                    session.Update(user);   //对数据库进行操作
                    transcation.Commit(); //提交事务
                }
            }
        }

        //验证登录
        public bool VerifyUser(string username, string password)
        {
            using (ISession session = MyNHibernateHelper.OpenSession()) //请求session
            {
                User user = session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Username", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();
                if(user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
}
