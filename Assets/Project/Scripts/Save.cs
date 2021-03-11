using System.Collections.Generic;
using Cngine;

namespace Flappy
{
    public class Save
    {
        public List<FlappyScoreData> _bestScoresSave;
        public List<FlappyScoreData> BestScoresSave
        {
            get
            {
                if (_bestScoresSave != null)
                {
                    return _bestScoresSave;
                }
                
                _bestScoresSave = DataStore.Load("BestScore",new List<FlappyScoreData>());
                return _bestScoresSave;
            }

            set
            {
                DataStore.Save<List<FlappyScoreData>>( value,"BestScore");
                _bestScoresSave = null;
            }
        }
    }
}