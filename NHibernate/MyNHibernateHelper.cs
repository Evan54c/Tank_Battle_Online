using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

namespace MyNHibernate
{
    class MyNHibernateHelper
    {
        private static ISessionFactory  _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();//解析nhibernate.cfg.xml
                    configuration.AddAssembly("MyNHibernate"); //解析映射文件 User.hbm.xml....

                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
;       }

    }
}
