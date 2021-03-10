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

                var data = DataStore.Load("BestScore", JsonSerializer.Serialize(new List<FlappyScoreData>()));
                _bestScoresSave = JsonSerializer.Deserialize<List<FlappyScoreData>>(data);
                return _bestScoresSave;
            }

            set
            {
                DataStore.Save("BestScore", JsonSerializer.Serialize(_bestScoresSave));
                _bestScoresSave = null;
            }
        }
    }
}