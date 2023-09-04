
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace IISTask
{
    public class IISNotifier : IRegisteredObject
    {
        public void Stop(bool immediate)
        {
            //do nothing, I only run tasks if I know that they won't take more than a few seconds.
        }

        public IISNotifier()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Finished()
        {
            HostingEnvironment.UnregisterObject(this);
        }
    }
}
namespace IISTask
{
    public class IISTaskFactory
    {
        public static Task StartNew(Action act)
        {
            IISNotifier notif = new IISNotifier();
            return Task.Factory.StartNew(() => {
                act.Invoke();
                notif.Finished();
            });
        }

        public static Task StartNew(Action act, TaskCreationOptions options)
        {
            IISNotifier notif = new IISNotifier();
            return Task.Factory.StartNew(() =>
            {
                act.Invoke();
                notif.Finished();
            }, options);
        }
    }
}