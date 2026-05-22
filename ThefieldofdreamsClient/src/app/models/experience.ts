export interface Experience {
  id: string;
  title: string;
  description?: string;
  imageUrl?: string;
  location?: string;
  price?: number;
  currency?: string;
  category?: string;
  tags?: string[];
  isFeatured: boolean;
  isActive: boolean;
  createdAt: string;
}
