export interface MessageApiModel {
  id: number;
  authorNumber: boolean;
  text: string;
  imageFileName: string;
  fileName: string;
  postedAt: Date;
  notViewed: boolean;
}
