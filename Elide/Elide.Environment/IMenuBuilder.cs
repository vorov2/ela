using System;

namespace Elide.Environment
{
    public interface IMenuBuilder<T> where T : new()
    {
        T Finish();

        IMenuBuilder<T> Separator();

        IMenuBuilder<T> Menu(string text);

        IMenuBuilder<T> Menu(string text, Action<Object> expandHandler);

        IMenuBuilder<T> CloseMenu();

        IMenuBuilder<T> Items(Action<IMenuBuilder<T>> addAct);

        IMenuBuilder<T> ItemsDynamic(Action<IMenuBuilder<T>> addAct);

        IMenuBuilder<T> Item(string text, Action handler, Func<Boolean> pred);

        IMenuBuilder<T> Item(string text, Action handler);

        IMenuBuilder<T> Item(string text, string keys, Action handler);

        IMenuBuilder<T> Item(string text, string keys, Action handler, Func<Boolean> pred);

        IMenuBuilder<T> Item(string text, string keys, Action handler, Func<Boolean> pred, Func<Boolean> checkPred);
    }
}
