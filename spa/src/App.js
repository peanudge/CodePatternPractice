import "./App.css";
import ReactFlow, { useNodesState, useEdgesState, addEdge } from "reactflow";
import React, { useCallback, SyntheticEvent, useEffect } from "react";
import "reactflow/dist/style.css";

const initialNodes = [
  { id: "1", position: { x: 0, y: 0 }, data: { label: "1" } },
  { id: "2", position: { x: 0, y: 100 }, data: { label: "2" } },
  { id: "3", position: { x: 0, y: 200 }, data: { label: "3" } },
];
const initialEdges = [
  { id: "e1-2", source: "1", target: "2" },
  { id: "e2-3", source: "2", target: "3" },
];

export default function App() {
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);

  const onConnect = useCallback(
    (params) => setEdges((eds) => addEdge(params, eds)),
    [setEdges]
  );
  const handleClick = (e) => {
    console.log(e.target);
  };

  useEffect(() => {
    let timer = null;

    timer = setInterval(() => {
      fetch("/api/graph")
        .then((res) => res.json())
        .then((data) => console.log(data))
        .catch((err) => {
          console.error(err);
        });
    }, 1000);

    return () => {
      clearInterval(timer);
    };
  }, []);

  return (
    <div style={{ width: "100vw", height: "100vh" }}>
      <p></p>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onClick={handleClick}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        onConnect={onConnect}
      />
    </div>
  );
}
