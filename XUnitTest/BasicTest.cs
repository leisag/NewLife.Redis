using System;
using System.Threading;
using NewLife.Caching;
using Xunit;

namespace XUnitTest
{
    public class BasicTest
    {
        public FullRedis Cache { get; set; }

        public BasicTest()
        {
            FullRedis.Register();
            var rds = FullRedis.Create("127.0.0.1:6379", 2);

            Cache = rds as FullRedis;
        }

        [Fact(DisplayName = "��Ϣ����", Timeout = 1000)]
        public void InfoTest()
        {
            var inf = Cache.Execute<String>(null, client => client.Execute<String>("info"));
            Assert.NotNull(inf);
        }

        [Fact(DisplayName = "�ַ�������")]
        public void GetSet()
        {
            var ic = Cache;
            var key = "Name";

            // ����ɾ��
            ic.Set(key, Environment.UserName);
            ic.Append(key, "_XXX");
            var name = ic.Get<String>(key);
            Assert.Equal(Environment.UserName + "_XXX", name);

            var name2 = ic.GetRange(key, 0, Environment.UserName.Length - 1);
            Assert.Equal(Environment.UserName, name2);

            ic.SetRange(key, name.Length - 2, "YY");
            var name3 = ic.Get<String>(key);
            Assert.Equal(Environment.UserName + "_XYY", name3);
        }
    }
}