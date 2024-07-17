import { Handle, NodeProps, Position } from "reactflow";
import { GraphNode } from "../App";

const portStyle = {
  width: "10px",
  height: "10px",
  borderRadius: "0px",
  background: "red",
};

export const ACTION_NODE_TYPE = "actionNode";
export function ActionNode(props: NodeProps<GraphNode>) {
  const data = props.data;
  const inputPorts = data.inputPorts;
  const outputPorts = data.outputPorts;
  return (
    <>
      <div
        style={{
          border: "2px solid black",
          borderRadius: "100%",
          background: "transparent",
          width: "100px",
          height: "100px",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <span>{data.name}</span>
        {inputPorts.map((port, idx) => {
          return (
            <Handle
              key={port.name}
              id={port.name}
              type="target"
              position={Position.Left}
              style={{ ...portStyle, top: "50%" }}
            />
          );
        })}

        {outputPorts.map((port, idx) => {
          return (
            <Handle
              key={port.name}
              id={port.name}
              type="source"
              position={Position.Right}
              style={{ ...portStyle, top: "50%" }}
            />
          );
        })}
      </div>
    </>
  );
}
