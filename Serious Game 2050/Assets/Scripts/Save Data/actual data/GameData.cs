[System.Serializable]
public class GameData
{
    
    
        public int money;
        


        //sets money to whatever value when u start new game
        //game starts like this when no save data is available or you create a new save file
        public GameData()
        {
            this.money = 0;
            

        }
    
}
