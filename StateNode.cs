using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore {
    public abstract class StateNode {
        //节点类型
        public virtual NodeTypeEnum NodeType => NodeTypeEnum.BasicNode;

        ///<summary>
        ///激活节点时调用，仅仅调用一次
        ///</summary>
        public virtual void OnEnter() {
        }

        ///<summary>
        ///准备节点所需信息，每次加载都会调用
        ///</summary>
        public virtual void OnPrepare() {
        }

        ///<summary>
        ///准备开始，每次加载都会调用，且一定在所有激活节点的prepare完成后才调用
        ///</summary>
        public virtual void OnBegin() {
        }
        
        ///<summary>
        ///退出节点时调用，仅仅调用一次
        ///</summary>
        public virtual void OnFinish() {
        }
    }

    public class OneOutNode : StateNode {

    }

    public class MultOutNode : StateNode {

    }

    public class StartNode : StateNode{
        public override NodeTypeEnum NodeType => NodeTypeEnum.StartNode;
    }

    public class EndNode : StateNode {
        public override NodeTypeEnum NodeType => NodeTypeEnum.EndNode;
    }

    class InPort {

    }

    class OutPort {

    }
}
