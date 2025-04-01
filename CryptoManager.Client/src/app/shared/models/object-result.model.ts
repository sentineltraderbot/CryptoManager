import { SimpleObjectResult } from "./simple-object-result.model";

export class ObjectResult<T> extends SimpleObjectResult {
  item: T;
}
