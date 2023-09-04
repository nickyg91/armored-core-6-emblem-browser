interface IKeyValuePair<T, K> {
  key: T;
  value: K;
}

export function toMapOfEnumDescriptions<T extends Object>(enumType: T): Map<string, number> {
  const map = new Map<string, number>();
  Object.keys(enumType)
    .filter((key) => isNaN(Number(key)))
    .map((key) => {
      map.set(key, Number(enumType[key as keyof typeof enumType]));
    });
  return map;
}

export function toArrayOfEnumDescriptions<T extends Object>(
  enumType: T
): IKeyValuePair<string, number>[] {
  const arr = new Array<IKeyValuePair<string, number>>();
  Object.keys(enumType)
    .filter((key) => isNaN(Number(key)))
    .map((key) => {
      arr.push({ key: key, value: Number(enumType[key as keyof typeof enumType]) } as IKeyValuePair<
        string,
        number
      >);
    });
  return arr;
}
