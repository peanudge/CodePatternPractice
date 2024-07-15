import { useCallback, useEffect, useState } from "react";
import ReactFlow, {
  addEdge,
  applyEdgeChanges,
  applyNodeChanges,
  Background,
  BackgroundVariant,
  Connection,
  Controls,
  MiniMap,
  NodeChange,
  OnEdgesChange,
  OnNodesChange,
  useEdgesState,
  useNodesState,
} from "reactflow";
import "reactflow/dist/style.css";
import "./App.css";

type NodePort = {
  name: string;
};
type Node = {
  id: string;
  name: string;
  inputPorts: NodePort[];
  outputPorts: NodePort[];
};
type NodeLink = {
  srcNodeId: string;
  srcPortName: string;
  destNodeId: string;
  destPortName: string;
};

type NodeGraph = {
  nodes: Node[];
  links: NodeLink[];
};

export default function App() {
  const [graph, setGraph] = useState<NodeGraph | undefined>(undefined);

  const [nodes, setNodes] = useNodesState([]);
  const [edges, setEdges] = useEdgesState([]);

  useEffect(() => {
    fetch("/api/graph")
      .then((res) => res.json())
      .then((data: NodeGraph) => {
        setGraph(data);

        const nodes = data.nodes.map((node, idx) => {
          return {
            id: node.id,
            position: {
              x: idx * 200,
              y: 0,
            },
            data: { label: node.name },
          };
        });
        setNodes(nodes);

        const edges = data.links.map((link) => {
          return {
            id: link.srcNodeId + "-" + link.destNodeId,
            source: link.srcNodeId,
            target: link.destNodeId,
            animated: true,
            label: "action",
            type: "step",
          };
        });
        setEdges(edges);
      })
      .catch((err) => {
        console.error(err);
      });
  }, [setEdges, setNodes]);

  const onConnect = (params: Connection) =>
    setEdges((eds) => addEdge(params, eds));

  const onNodeChange: OnNodesChange = useCallback(
    (changes: NodeChange[]) => {
      if (changes.length > 0 && changes[0].type === "position") {
        console.log("Node Change: ", changes[0].position);
      }
      setNodes((nds) => applyNodeChanges(changes, nds));
    },
    [setNodes]
  );
  const onEdgesChanges: OnEdgesChange = useCallback(
    (changes) => {
      setEdges((eds) => applyEdgeChanges(changes, eds));
    },
    [setEdges]
  );

  return (
    <div className="App">
      <div
        style={{
          height: "100vh",
          width: "50vw",
        }}
      >
        <ReactFlow
          nodes={nodes}
          edges={edges}
          onClick={(e) => {
            console.log(e.target);
          }}
          onNodesChange={onNodeChange}
          onEdgesChange={onEdgesChanges}
          onConnect={onConnect}
          fitView
          panOnScroll={true}
        >
          <Background color="#654d4d" variant={BackgroundVariant.Dots} />
          <MiniMap
            nodeColor={(node) => {
              switch (node.type) {
                case "normal":
                  return "red";
              }
              return "black";
            }}
            nodeStrokeWidth={3}
          />
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
