package toolgood.flowVision.Engines.OutDatas;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

public class StockNode {
    private final List<ChannelNode> Nodes = new ArrayList<>();
    private final HashSet<String> IdSet = new HashSet<String>();

    public void Push(TreeNode node, String channel, int index) {
        if (IdSet.add(node.CurrWork.Id)) {// 防止合并
            ChannelNode nd = new ChannelNode();
            nd.Channel = channel;
            nd.Node = node;
            nd.Index = index;
            Nodes.add(nd);
        }
    }

    public boolean IsNotNull() {
        return Nodes.size() != 0;
    }

    public ChannelNode Pop() {
        if (Nodes.size() == 0) {
            return null;
        }

        ChannelNode temp = Nodes.get(0);
        for (int i = 1; i < Nodes.size(); i++) {
            ChannelNode channelNode = Nodes.get(i);
            if (temp != null) {
                if (channelNode.Node.Layer() < temp.Node.Layer()) {
                    temp = channelNode;
                }
            } else {
                temp = channelNode;
            }
        }
        ChannelNode node = temp;
        if (temp != null) {
            Nodes.remove(temp);
            IdSet.remove(temp.Node.Id());
        }
        return node;
    }

}
