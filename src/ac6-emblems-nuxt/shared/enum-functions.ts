interface IKeyValuePair<T, K> {
  key: T;
  value: K;
}

export function toMapOfEnumDescriptions<T extends Object>(enumType: T): Map<string, number> {
  const map = new Map<string, number>();
  Object.keys(enumType)
    .filter((key) => isNaN(Number(key)))
    // eslint-disable-next-line array-callback-return
    .map((key) => {
      map.set(key, Number(enumType[key as keyof typeof enumType]));
    });
  return map;
}

export function toArrayOfEnumDescriptions<T extends Object>(enumType: T): IKeyValuePair<string, number>[] {
  const arr = new Array<IKeyValuePair<string, number>>();
  Object.keys(enumType)
    .filter((key) => isNaN(Number(key)))
    // eslint-disable-next-line array-callback-return
    .map((key) => {
      arr.push({ key, value: Number(enumType[key as keyof typeof enumType]) } as IKeyValuePair<string, number>);
    });
  return arr;
}
