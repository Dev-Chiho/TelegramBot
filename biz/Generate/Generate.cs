using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomBot.biz.Interface;
using CustomBot.Enums;

namespace CustomBot.biz.Generate
{
    public class Generate
    {
        private static Generate _instance;
        private Generate()
        {
        }

        public static Generate Instance()
        {
            if (_instance == null)
            {
                _instance = new Generate();
            }

            return _instance;
        }

        /// <summary>
        /// 프로세스 실행 구문
        /// </summary>
        /// <param name="behavior"></param>
        public void Excute(Behavior behavior)
        {
            IFunction iFunction = Factory.Instance(behavior);

            switch (behavior)
            {
                case Behavior.Telegram:
                    iFunction.Run_Bot();
                    break;
            }
        }
    }
}

