export interface Game {
  id: string;
  title: string;
  description?: string;
  imageUrl?: string;
  isActive: boolean;
  createdAt: string;
}
