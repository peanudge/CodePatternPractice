import { useCallback, useEffect, useState } from "react";
import ReactFlow, {
  addEdge,
  Background,
  BackgroundVariant,
  Connection,
  Controls,
  Edge,
  MarkerType,
  MiniMap,
  Node,
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

type GraphProcesssingInfo = {
  currentRunningNodeIds: string[];
  isRunning: boolean;
  isEnd: boolean;
};

function createReactFlowNode(data: GraphNode, idx: number): Node<GraphNode> {
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

function createReactFlowEdge(data: GraphNodeLink): Edge<GraphNodeLink> {
  return {
    id: data.srcNodeId + "-" + data.destNodeId,
    source: data.srcNodeId,
    target: data.destNodeId,
    sourceHandle: data.srcPortName,
    targetHandle: data.destPortName,
    animated: false,
    type: "default",
    markerEnd: { type: MarkerType.ArrowClosed },
    data,
  };
}

const nodeTypes = { [ACTION_NODE_TYPE]: ActionNode };

export default function App() {
  const [graph, setGraph] = useState<NodeGraph | undefined>(undefined);

  const [graphTextData, setGraphTextData] = useState<string>("");
  const [error, setError] = useState<string>("");

  const [nodes, setNodes, onNodesChange] = useNodesState<GraphNode>([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState<GraphNodeLink>([]);

  const [graphProcessingInfo, setGraphProcessingInfo] = useState<
    GraphProcesssingInfo | undefined
  >(undefined);

  const refreshGraphData = (data: NodeGraph) => {
    setGraph(data);
    setGraphTextData(JSON.stringify(data, null, 2));
    setNodes(data.nodes.map(createReactFlowNode));
    setEdges(data.links.map(createReactFlowEdge));
  };

  useEffect(() => {
    fetch("/api/graph")
      .then((res) => res.json())
      .then((data: NodeGraph) => {
        setGraph(data);
        setGraphTextData(JSON.stringify(data, null, 2));
        setNodes(data.nodes.map(createReactFlowNode));
        setEdges(data.links.map(createReactFlowEdge));
      })
      .catch((err) => {
        console.error(err);
      });
  }, [setEdges, setNodes]);

  const highlightRunningNode = useCallback(
    (runningNodeIds: string[]) => {
      setNodes((nds) =>
        nds.map((nd) => {
          if (runningNodeIds.includes(nd.data.id)) {
            return {
              ...nd,
              style: { background: "#00e676" },
            };
          } else {
            return {
              ...nd,
              style: { background: "white" },
            };
          }
        })
      );
    },
    [setNodes]
  );

  useEffect(() => {
    let intervalTimerId = setInterval(async () => {
      const res = await fetch("/api/graph/process", {
        method: "GET",
      });

      if (!res.ok) {
        return;
      }

      const data = (await res.json()) as {
        graphProcessor: GraphProcesssingInfo;
      };

      if (data.graphProcessor === null) {
        setGraphProcessingInfo(undefined);
        return;
      }

      setGraphProcessingInfo(data.graphProcessor);

      const runningNodeIds = data.graphProcessor.currentRunningNodeIds ?? [];
      highlightRunningNode(runningNodeIds);
    }, 500);

    return () => {
      clearInterval(intervalTimerId);
    };
  }, [highlightRunningNode]);

  const onConnect = (newConnection: Connection) =>
    setEdges((eds) => {
      // target == input
      // source == output
      const linksConnectedInputPort = eds.filter(
        (ed) =>
          ed.target === newConnection.target &&
          ed.targetHandle === newConnection.targetHandle
      );

      if (linksConnectedInputPort.length !== 0) {
        alert("Target Input Port Already Connected");
        return eds;
      }

      const duplicated = linksConnectedInputPort.some(
        (ed) =>
          ed.source === newConnection.source &&
          ed.sourceHandle === newConnection.sourceHandle
      );

      if (duplicated) {
        alert("This is duplicated Connection");
        return eds;
      }

      const newEdge = createReactFlowEdge({
        srcNodeId: newConnection.source!,
        srcPortName: newConnection.sourceHandle!,
        destNodeId: newConnection.target!,
        destPortName: newConnection.targetHandle!,
      });
      return addEdge(newEdge, eds);
    });

  return (
    <div className="App">
      <div className="AppLeft">
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
          position: "relative",
        }}
      >
        <div style={{ height: "800px" }}>
          <textarea
            style={{
              wordBreak: "keep-all",
              fontSize: "18px",
              width: "100%",
              height: "100%",
            }}
            value={graphTextData}
            onChange={(e) => {
              setGraphTextData(e.target.value);
            }}
          />
        </div>

        {error && <p style={{ color: "red" }}>{error}</p>}
        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            gap: "10px",
            marginTop: "10px",
          }}
        >
          <button
            onClick={() => {
              try {
                const data = JSON.parse(graphTextData);
                setGraph(data);
                setNodes(data.nodes.map(createReactFlowNode));
                setEdges(data.links.map(createReactFlowEdge));
                setError("");
              } catch (err) {
                setError((err as Error).message);
              }
            }}
          >
            Update Only Graph
          </button>
          <button
            onClick={async () => {
              try {
                const parsedGraphdata = JSON.parse(graphTextData);
                const res = await fetch("/api/graph", {
                  method: "POST",
                  headers: {
                    "Content-Type": "application/json",
                  },
                  body: JSON.stringify(parsedGraphdata),
                });
                if (!res.ok) {
                  throw new Error("Failed to upload graph to server");
                } else {
                  const data = (await res.json()) as NodeGraph;
                  refreshGraphData(data);
                }
                setError("");
              } catch (err) {
                setError((err as Error).message);
              }
            }}
          >
            Upload Graph To Server
          </button>
          <button
            onClick={() => {
              setGraphTextData(JSON.stringify(graph, null, 2));
            }}
          >
            Reset
          </button>
          <button>Load Example Code</button>
        </div>
        <div
          style={{
            marginTop: "20px",
            display: "flex",
            gap: "20px",
          }}
        >
          <button onClick={() => startGraphProcessing(1000, 300)}>
            Start Graph Processing
          </button>
          <span>
            {graphProcessingInfo
              ? graphProcessingInfo.isRunning
                ? "Graph Processing Running..."
                : "Graph Processing End!"
              : "Not Started"}
          </span>
        </div>
      </div>
    </div>
  );
}

export const startGraphProcessing = async (
  roundInterval: number,
  nodeOperationDelay: number
) => {
  const res = await fetch(
    `api/graph/process/start?roundInterval=${roundInterval}&nodeOperationDelay=${nodeOperationDelay}`,
    {
      method: "PUT",
    }
  );

  if (res.ok) {
    return true;
  } else {
    return false;
  }
};
