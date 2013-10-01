using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public abstract class AbstractCodeRunnerService : Service, ICodeRunnerService
    {
        protected AbstractCodeRunnerService()
        {
            RunnerInstances = new Dictionary<Type,Object>();
        }

        public virtual bool RunCode<T>(T compiled, ExecOptions options, params ExtendedOption[] extOptions) where T : ICompiledAssembly
        {
            var type = typeof(T);
            var runnerObj = default(Object);

            if (type == typeof(ICompiledAssembly))
                type = compiled.GetType();

            if (!RunnerInstances.TryGetValue(type, out runnerObj))
                throw new ElideException("Unable to find runner for '{0}'.", type);

            var runner = (ICodeRunner<T>)runnerObj;
            runner.App = App;
            return Run(compiled, options, runner, extOptions);
        }

        public abstract bool IsRunning();

        public abstract bool AbortExecution();

        protected abstract bool Run<T>(T compiled, ExecOptions options, ICodeRunner<T> runner, params ExtendedOption[] extOptions) where T : ICompiledAssembly;
        
        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Runners.ForEach(b =>
            {
                var iface = default(Type);

                if ((iface = b.Type.GetInterface(typeof(ICodeRunner<>).FullName)) == null)
                    throw new ElideException("Runner '{0}' doesn't implement ICodeRunner<> interface.", b.Type);

                var arg = iface.GetGenericArguments()[0];
                RunnerInstances.Add(arg, TypeCreator.New(b.Type));
            });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new CodeRunnerReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Runners.ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Runners.FirstOrDefault(r => r.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "runners")
                throw new ElideException("Section '{0}' is not supported by CodeRunnerService.", section);
        }

        protected internal IEnumerable<CodeRunnerInfo> Runners { get; internal set; }

        protected Dictionary<Type,Object> RunnerInstances { get; private set; }
    }
}
