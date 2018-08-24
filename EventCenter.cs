using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore {
    class EventCenter {
        #region public
        //单例
        public static readonly EventCenter Instance;

        //提供的4种事件触发类型
        public delegate void ActionWithNoPara();
        public delegate void ActionWithPara1<T>(T para);
        public delegate void ActionWithPara2<T1, T2>(T1 para1, T2 para2);
        public delegate void ActionWithParaS<T>(T[] paras);

        //事件的注册与信号的发送
        //无参数事件
        public void RegisterEventWithNoPara(EventSignalEnum eventSignal, Object tag, ActionWithNoPara actionWithNoPara) {
            RegisterEvent(eventSignal, tag, mEventDict0, actionWithNoPara);
        }
        public void SendSignalWithNoPara(EventSignalEnum eventSignal) {
            if (mEventDict0.ContainsKey(eventSignal)) {
                foreach (ActionWithNoPara action in mEventDict0[eventSignal]){
                    action();
                }
            }
        }

        //单参数事件
        public void RegisterEventWithPara1<T>(EventSignalEnum eventSignal, Object tag, ActionWithPara1<T> actionWithPara1) {
            RegisterEvent(eventSignal, tag, mEventDict1, actionWithPara1);
        }
        public void SendSignalWithPara1<T>(EventSignalEnum eventSignal, T para) {
            if (mEventDict1.ContainsKey(eventSignal)) {
                foreach (Object action in mEventDict1[eventSignal]) {
                    ActionWithPara1<T> actionWithPara1 = action as ActionWithPara1<T>;
                    if (actionWithPara1 != null)
                        actionWithPara1(para);
                }
            }
        }

        //双参数事件
        public void RegisterEventWithPara2<T1, T2>(EventSignalEnum eventSignal, Object tag, ActionWithPara2<T1, T2> actionWithPara2) {
            RegisterEvent(eventSignal, tag, mEventDict2, actionWithPara2);
        }
        public void SendSignalWithPara2<T1, T2>(EventSignalEnum eventSignal, T1 para1, T2 para2) {
            if (mEventDict2.ContainsKey(eventSignal)) {
                foreach (Object action in mEventDict2[eventSignal]) {
                    ActionWithPara2<T1, T2> actionWithPara2 = action as ActionWithPara2<T1, T2>;
                    if (actionWithPara2 != null)
                        actionWithPara2(para1, para2);
                }
            }
        }

        //任意参数事件
        public void RegisterEventWithParaS<T>(EventSignalEnum eventSignal, Object tag, ActionWithParaS<T> actionWithParaS) {
            RegisterEvent(eventSignal, tag, mEventDictS, actionWithParaS);
        }
        public void SendSignalWithParaS<T>(EventSignalEnum eventSignal, params T[] paras) {
            if (mEventDictS.ContainsKey(eventSignal)) {
                foreach (Object action in mEventDictS[eventSignal]) {
                    ActionWithParaS<T> actionWithParaS = action as ActionWithParaS<T>;
                    if (actionWithParaS != null)
                        actionWithParaS(paras);
                }
            }
        }

        //取消注册
        public void RemoveEvent(Object tag) {
            if (mActionTagDict.ContainsKey(tag)) {
                foreach (ActionHolder actionHolder in mActionTagDict[tag]) {
                    actionHolder.actionList.Remove(actionHolder.action);
                }
                mActionTagDict.Remove(tag);
            }
        }

        //清理空闲索引
        public void CleanIndex() {
            CleanDictIndex(mEventDict0);
            CleanDictIndex(mEventDict1);
            CleanDictIndex(mEventDict2);
            CleanDictIndex(mEventDictS);
        }

        //用于生成独立的Tag
        public class EventTag {
        }
        #endregion

        #region private
        //静态构造初始化单例
        static EventCenter() {
            Instance = new EventCenter();
        }

        //注册的事件
        private Dictionary<EventSignalEnum, List<Object>> mEventDict0 = new Dictionary<EventSignalEnum, List<Object>>();
        private Dictionary<EventSignalEnum, List<Object>> mEventDict1 = new Dictionary<EventSignalEnum, List<Object>>();
        private Dictionary<EventSignalEnum, List<Object>> mEventDict2 = new Dictionary<EventSignalEnum, List<Object>>();
        private Dictionary<EventSignalEnum, List<Object>> mEventDictS = new Dictionary<EventSignalEnum, List<Object>>();

        //用于取消注册
        private class ActionHolder {
            //保存事件的列表
            public List<Object> actionList;
            //保存的事件
            public Object action;
            //构造
            public ActionHolder(List<Object> list, Object action) {
                this.actionList = list;
                this.action = action;
            }
        }
        private Dictionary<Object, List<ActionHolder>> mActionTagDict = new Dictionary<object, List<ActionHolder>>();

        //清理索引的辅助函数
        private void CleanDictIndex(Dictionary<EventSignalEnum, List<Object>> eventDict) {
            List<EventSignalEnum> listToRemove = new List<EventSignalEnum>();
            foreach (KeyValuePair<EventSignalEnum, List<Object>> eventPair in eventDict) {
                if (eventPair.Value.Count == 0) {
                    listToRemove.Add(eventPair.Key);
                }
            }
            foreach (EventSignalEnum eventSignal in listToRemove) {
                eventDict.Remove(eventSignal);
            }
        }

        //记录对应Tag的事件
        private void AddToTagList(Object tag, List<Object> list, Object action) {
            if (mActionTagDict.ContainsKey(tag)) {
                mActionTagDict[tag].Add(new ActionHolder(list, action));
            } else {
                List<ActionHolder> actionHolderList = new List<ActionHolder>();
                actionHolderList.Add(new ActionHolder(list, action));
                mActionTagDict.Add(tag, actionHolderList);
            }
        }

        //注册辅助函数
        private void RegisterEvent(EventSignalEnum eventSignal, Object tag, Dictionary<EventSignalEnum, List<Object>> eventDict, Object action) {
            List<Object> actionList = null;
            if (eventDict.ContainsKey(eventSignal)) {
                actionList = eventDict[eventSignal];
            } else {
                actionList = new List<Object>();
                eventDict.Add(eventSignal, actionList);
            }
            actionList.Add(action);
            AddToTagList(tag, actionList, action);
        }
        #endregion

        #region test
        public static void Main() {
            EventCenter eventCenter = EventCenter.Instance;
            EventTag testTag1 = new EventTag();
            EventTag testTag2 = new EventTag();
            //注册
            eventCenter.RegisterEventWithNoPara(EventSignalEnum.Default, testTag1, test0);
            eventCenter.RegisterEventWithPara1<int>(EventSignalEnum.Default, testTag1, test1);
            eventCenter.RegisterEventWithPara1<string>(EventSignalEnum.Default, testTag1, test11);
            eventCenter.RegisterEventWithPara2<float, float>(EventSignalEnum.Default, testTag1, test2);
            eventCenter.RegisterEventWithParaS<string>(EventSignalEnum.Default, testTag1, test3);
            eventCenter.RegisterEventWithParaS<string>(EventSignalEnum.Test, testTag2, test3);

            //发送信号
            eventCenter.SendSignalWithNoPara(EventSignalEnum.Default);
            eventCenter.SendSignalWithPara1(EventSignalEnum.Default, 1);
            eventCenter.SendSignalWithPara1(EventSignalEnum.Default, "test11");
            eventCenter.SendSignalWithPara2(EventSignalEnum.Default, 2f, 3f);
            eventCenter.SendSignalWithParaS(EventSignalEnum.Default, "testTag1");
            eventCenter.SendSignalWithParaS(EventSignalEnum.Test,"This is ", "testTag2");

            //取消注册
            eventCenter.RemoveEvent(testTag1);
            //测试
            eventCenter.SendSignalWithParaS(EventSignalEnum.Default, "testTag1");
            eventCenter.SendSignalWithParaS(EventSignalEnum.Test, "This is ", "testTag2");

            //取消注册
            eventCenter.RemoveEvent(testTag2);
            //测试
            eventCenter.SendSignalWithParaS(EventSignalEnum.Default, "testTag1");
            eventCenter.SendSignalWithParaS(EventSignalEnum.Test, "This is ", "testTag2");
        }

        private static void test0() {
            Console.WriteLine("test0");
        }

        private static void test1(int a) {
            Console.WriteLine("test1 " + a);
        }

        private static void test11(string a) {
            Console.WriteLine("test11 " + a);
        }

        private static void test2(float a, float b) {
            Console.WriteLine("tset2 " + a + " - " + b);
        }

        private static void test3(params string[] s) {
            StringBuilder result = new StringBuilder("test3 ");
            foreach (string ss in s) {
                result.Append(ss).Append(" - ");
            }
            Console.WriteLine(result.ToString());
        }
        #endregion
    }
}
