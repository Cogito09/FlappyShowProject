using System.Collections.Generic;
using Cngine;

namespace Flappy
{
    public class Save : DataObject<SaveData>
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
            
            set => DataStore.Save("BestScore",JsonSerializer.Serialize(_bestScoresSave)) ;
        }
        
        protected Save(SaveData model, bool dontWarnAboutMissingData = false) : base(model, dontWarnAboutMissingData)
        {
        }
    }
}