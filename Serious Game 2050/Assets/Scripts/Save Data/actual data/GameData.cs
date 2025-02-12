[System.Serializable]
public class GameData
{
    
    
        public int money;
        public int wood;
        public int energy;
        public int minerals;
        public int water;
        public int mileau;


        //sets money to whatever value when u start new game
        //game starts like this when no save data is available or you create a new save file
        public GameData()
        {
            this.money = 0;
            this.wood = 0;
            this.energy = 0;
            this.minerals = 0;
            this.water = 0;
            this.mileau = 0;

        }
    
}
