import { Handle, NodeProps, Position } from "reactflow";
import { GraphNode } from "../App";

const portStyle = {
  width: "15px",
  height: "15px",
  background: "#c0c0c0",
  border: "1px solid black",
};

const inputPortStyle = {
  ...portStyle,
  borderRadius: "100%",
};

const outputPortStyle = {
  ...portStyle,
  borderRadius: "0px",
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
          borderRadius: "8px",
          background: "transparent",
          width: "100px",
          height: "100px",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <span>{data.name}</span>

        <div
          style={{
            position: "absolute",
            left: "-15px",
            display: "flex",
            flexDirection: "column",
            gap: "4px",
          }}
        >
          {inputPorts.map((port, idx) => {
            return (
              <Handle
                key={port.name}
                id={port.name}
                type="target"
                position={Position.Left}
                style={{ ...inputPortStyle, position: "relative" }}
              />
            );
          })}
        </div>
        <div
          style={{
            position: "absolute",
            right: "-15px",
            display: "flex",
            flexDirection: "column",
            gap: "4px",
          }}
        >
          {outputPorts.map((port, idx) => {
            return (
              <Handle
                key={port.name}
                id={port.name}
                type="source"
                position={Position.Right}
                style={{ ...outputPortStyle, position: "relative" }}
              />
            );
          })}
        </div>
      </div>
    </>
  );
}
