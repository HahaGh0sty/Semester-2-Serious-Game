[System.Serializable]
public class GameData
{
    
    
        public int money;
        public int wood;
        public int stone;
        public int energy;
        public int water;
        public int vervuiling;
        


        //sets resources to whatever value when u start new game
        //game starts like this when no save data is available or you create a new save file
        public GameData()
        {
            this.money = 0;
            this.wood = 0;
            this.stone = 0;
            this.energy = 0;
            this.water = 0;
            this.vervuiling = 0;
            

        }
    
}
