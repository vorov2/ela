using System;
using System.Collections.Generic;
using System.Linq;

namespace Elide.Core
{
    public abstract class AbstractApp : IApp
    {
        protected AbstractApp(IEnumerable<ExtSection> sections)
        {
            Sections = sections;
        }

        public virtual void Run()
        {
            ServiceInstances
                .Where(kv => kv.Value is IExecService)
                .Select(kv => new { a = Attr.Get<ExecOrderAttribute>(GetServiceType(kv.Key)), s = (IExecService)kv.Value })
                .OrderBy(t => t.a.Order)
                .ForEach(t => t.s.Run());
        }

        private Type GetServiceType(Type iface)
        {
            return Services.First(s => s.InterfaceType == iface).Type;
        }

        public void Initialize(IApp app)
        {
            ServiceInstances = new Dictionary<Type,IService>();
            Services.ForEach(ValidateService);
            Services.ForEach(s => ServiceInstances.Add(s.InterfaceType, Activator.CreateInstance(s.Type) as IService));
            Services.Where(s => s.HasRegSections()).ForEach(InitializeRegService);
            
            var queue = new Queue<ServiceInfo>(Services);

            while (queue.Count != 0)
            {
                var s = queue.Dequeue();
                var attrs = Attribute.GetCustomAttributes(s.Type, typeof(DependsFromAttribute)).OfType<DependsFromAttribute>();

                if (attrs.Any(a => queue.Any(si => si.InterfaceType == a.ServiceType)))
                    queue.Enqueue(s);
                else
                    ServiceInstances[s.InterfaceType].Initialize(this);
            }

            OnInitialize();
        }

        public void Unload()
        {
            try
            {
                ServiceInstances.Values.ForEach(s => s.Unload());
                OnUnload();
            }
            catch { }
        }

        private void ValidateService(ServiceInfo info)
        {
            if (info.InterfaceType.GetInterface(typeof(IService).FullName) == null)
                throw new ElideException("Service type should be derived from IService.");

            if (info.Type.GetInterface(info.InterfaceType.FullName) == null)
                throw new ElideException("Service type should be derived from {0}.", info.InterfaceType);
        }

        private void InitializeRegService(ServiceInfo info)
        {
            var serv = ServiceInstances[info.InterfaceType] as IExtService;

            if (serv == null)
                throw new ElideException("Service '{0}' doesn't implement IRegService interface.", info.Type);

            info.RegSections.ForEach(s => serv.CreateExtReader(s).Read(Sections.First(se => se.Name == s)));            
        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual void OnUnload()
        {

        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new ServiceReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Services.OfType<ExtInfo>();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Services.FirstOrDefault(s => s.Key == key);
        }

        public IService GetService(Type serviceType)
        {
            var serv = default(IService);

            if (!ServiceInstances.TryGetValue(serviceType, out serv))
                throw new ElideException("Service '{0}' not found.", serviceType);

            return serv;
        }

        public abstract IEnumerable<String> GetCommandLineArguments();

        private void ValidateSection(string section)
        {
            if (section != "services")
                throw new ElideException("Reg section '{0}' is not supported by App.", section);
        }

        protected internal IEnumerable<ServiceInfo> Services { get; internal set; }

        protected Dictionary<Type,IService> ServiceInstances { get; private set; }

        protected IEnumerable<ExtSection> Sections { get; private set; }
    }
}
