import { useEffect, useState } from "react";
import ReactFlow, {
  addEdge,
  Background,
  BackgroundVariant,
  Connection,
  Controls,
  MiniMap,
  useEdgesState,
  useNodesState,
} from "reactflow";
import "reactflow/dist/style.css";
import "./App.css";
import { ACTION_NODE_TYPE, ActionNode } from "./custom-nodes/ActionNode";

type GraphNodePort = {
  name: string;
};
export type GraphNode = {
  id: string;
  name: string;
  inputPorts: GraphNodePort[];
  outputPorts: GraphNodePort[];
};
type GraphNodeLink = {
  srcNodeId: string;
  srcPortName: string;
  destNodeId: string;
  destPortName: string;
};

type NodeGraph = {
  nodes: GraphNode[];
  links: GraphNodeLink[];
};

function createReactFlowNode(data: GraphNode, idx: number) {
  return {
    id: data.id,
    position: {
      x: idx * 200,
      y: 0,
    },
    type: ACTION_NODE_TYPE,
    data,
  };
}

function createReactFlowEdge(data: GraphNodeLink) {
  return {
    id: data.srcNodeId + "-" + data.destNodeId,
    source: data.srcNodeId,
    target: data.destNodeId,
    sourceHandle: data.srcPortName,
    targetHandle: data.destPortName,
    animated: true,
    type: "step",
  };
}
const nodeTypes = { [ACTION_NODE_TYPE]: ActionNode };

export default function App() {
  const [graph, setGraph] = useState<NodeGraph | undefined>(undefined);

  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  useEffect(() => {
    fetch("/api/graph")
      .then((res) => res.json())
      .then((data: NodeGraph) => {
        setGraph(data);
        setNodes(data.nodes.map(createReactFlowNode));
        setEdges(data.links.map(createReactFlowEdge));
      })
      .catch((err) => {
        console.error(err);
      });
  }, [setEdges, setNodes]);

  const onConnect = (params: Connection) =>
    setEdges((eds) => {
      return addEdge(params, eds);
    });

  return (
    <div className="App">
      <div
        style={{
          height: "100vh",
          width: "50vw",
        }}
      >
        <ReactFlow
          nodeTypes={nodeTypes}
          nodes={nodes}
          edges={edges}
          onClick={(e) => {}}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onConnect={onConnect}
          fitView
          panOnScroll={true}
        >
          <Background color="#654d4d" variant={BackgroundVariant.Dots} />
          <MiniMap nodeColor={() => "red"} nodeStrokeWidth={3} />
          <Controls showZoom showFitView />
        </ReactFlow>
      </div>
      <div
        className="AppRight"
        style={{
          backgroundColor: "#ccc5",
        }}
      >
        <pre
          style={{
            wordBreak: "keep-all",
            fontSize: "18px",
            margin: 0,
            padding: "14px",
          }}
        >
          {JSON.stringify(graph, null, 2)}
        </pre>
      </div>
    </div>
  );
}
