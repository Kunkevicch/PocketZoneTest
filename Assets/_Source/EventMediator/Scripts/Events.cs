namespace PocketZoneTest
{
    public struct EnemyInSightEvent
    {
        public Enemy Enemy { get; private set; }
        public EnemyInSightEvent(Enemy enemy)
        {
            Enemy = enemy;
        }
    }

    public struct EnemyOutSightEvent
    {
        public Enemy Enemy { get; private set; }

        public EnemyOutSightEvent(Enemy enemy)
        {
            Enemy = enemy;
        }
    }

    public struct AllEnemiesOutSightEvent { }

}
