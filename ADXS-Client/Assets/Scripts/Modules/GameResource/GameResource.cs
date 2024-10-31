namespace Assets.Scripts.Entity
{
    public class GameResource
    {
        // 资源属性
        public int gold { get; private set; }
        public int wood { get; private set; }
        public int population { get; private set; }

        public void RefreshGameResources()
        {

        }

        public void SetDefault()
        {
            gold = 500;
            wood = 150;
            population = 5;
        }
    }

}
