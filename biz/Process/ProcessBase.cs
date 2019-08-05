using CustomBot.biz.Interface;
using CustomBot.Enums;

namespace CustomBot.biz.Process
{
    /// <summary>
    /// 프로세스 베이스 페이지
    /// </summary>
    public class ProcessBase : IFunction
    {
        public ProcessCommon common;
        public Behavior baseBehavior;

        /// <summary>
        /// 베이스 페이지 생성자
        /// 기본값들 설정 및 초기화 합니다.
        /// </summary>
        public ProcessBase()
        {
            common = ProcessCommon.Instance;
        }

        public Behavior GetBehavior()
        {
            return baseBehavior;
        }

        /// <summary>
        /// 텔레그램 봇 기능
        /// </summary>
        public virtual void Run_Bot()
        {

        }
    }
}
