using BlazorApp8.Features;
using TimeWarp.State;

namespace BlazorApp8
{
    public class BaseComponent : TimeWarpStateComponent
    {
        internal CounterState CounterState => GetState<CounterState>();
    }
}
