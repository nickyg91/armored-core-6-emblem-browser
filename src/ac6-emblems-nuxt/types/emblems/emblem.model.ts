import type { PlatformType } from "../platform-type.enum";

export class Emblem {
  name!: string;
  platform!: PlatformType;
  shareId!: string;
  fileName?: string;
  imageUrl?: string;
  createdAtUtc!: Date;
  id!: number;
  imageData!: string;
  imageExtension!: string;
  tags!: string[];
}
