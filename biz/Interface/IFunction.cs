using CustomBot.Enums;

namespace CustomBot.biz.Interface
{
    public interface IFunction
    {
        /// <summary>
        /// 선택된 행위에 대한 값 가져오기
        /// </summary>
        /// <returns></returns>
        Behavior GetBehavior();

        /// <summary>
        /// 텔레그램 봇
        /// </summary>
        void Run_Bot();
    }
}
