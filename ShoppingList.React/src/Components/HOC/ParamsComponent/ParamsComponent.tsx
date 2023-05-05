import React from "react";
import { Params, useParams } from "react-router-dom";

type ParamsType<P> = Readonly<[P] extends [string] ? Params<P> : Partial<P>>;
type useParamsDefaultType = string | Record<string, string | undefined>;

export type ParamsComponentProps<T extends Readonly<unknown>, P extends useParamsDefaultType>
  = T & { params: ParamsType<P> };

export class ParamsComponent<T extends Readonly<unknown>, P extends useParamsDefaultType, S>
  extends React.Component<ParamsComponentProps<T, P>, S> { };

export function paramsHOC<T extends Readonly<unknown>, P extends useParamsDefaultType, S>(Cmp: typeof ParamsComponent<T, P, S>) {
  return (props: T) => {
    return <Cmp {...props} params={useParams<P>()} />
  }
}