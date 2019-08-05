using System;
using System.Collections.Generic;
using CustomBot.biz.Interface;
using CustomBot.biz.Process;
using CustomBot.Enums;

namespace CustomBot.biz.Generate
{
    public class Factory
    {
        private List<IFunction> _function = new List<IFunction>();
        public static Factory _this = new Factory();

        /// <summary>
        /// 객체 재성성을 막기위한 싱글톤 처리
        /// </summary>
        public static IFunction Instance(Behavior behavior)
        {
            IFunction process = null;

            foreach (IFunction _behavior in _this._function)
            {
                if ((Behavior)Enum.Parse(typeof(Behavior), behavior.ToString()) == _behavior.GetBehavior())
                {
                    process = _behavior;
                    break;
                }
            }

            if (process == null)
            {
                switch (behavior)
                {
                    case Behavior.Telegram:
                        process = new ProcessTelegram();
                        break;
                }

                _this._function.Add(process);
            }

            return process;
        }
    }
}

