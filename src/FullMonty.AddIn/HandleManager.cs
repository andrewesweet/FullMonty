namespace FullMonty.AddIn
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class HandleManager
    {
        private readonly IDictionary<string, Handle> handles = new ConcurrentDictionary<string, Handle>();

        public Handle this[string name]
        {
            get
            {
                if (handles.TryGetValue(name, out var handle)) return handle;

                throw new UnregisteredHandleException(name);
            }
        }

        public Handle Register(object payload)
        {
            return Register(payload, Guid.NewGuid().ToString());
        }

        public Handle Register(object payload, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name), "Handle name cannot be null");

            var handle = new Handle(payload, name);
            handles[handle.Name] = handle;
            return handle;
        }

        public class UnregisteredHandleException : Exception
        {
            public UnregisteredHandleException(string name) : base($"Handle '{name}' is not registered")
            {
            }
        }

        public class Handle
        {
            internal Handle(object payload, string name)
            {
                Payload = payload;
                Name = name;
            }

            public string Name { get; }

            public object Payload { get; }

            public T GetPayloadOrThrow<T>(string expectedType)
            {
                if (Payload is T t)
                {
                    return t;
                }

                throw new WrongHandleTypeException(Name, expectedType);
            }

            public T GetPayloadOrThrow<T>()
            {
                return GetPayloadOrThrow<T>(nameof(T));
            }
        }
    }
}
