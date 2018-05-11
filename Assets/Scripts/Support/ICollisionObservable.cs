namespace Simulation.Support
{
    interface ICollisionObservable
    {
        void ObserveCollisions(ICollisionObserver observer);
    }
}