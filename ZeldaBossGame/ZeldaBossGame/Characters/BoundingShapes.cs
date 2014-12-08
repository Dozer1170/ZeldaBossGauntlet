using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class BoundingShapes
    {
        public BoundingBox outerBoundingBox;
        private List<BoundingBox> innerBoundingBoxes;
        private List<BoundingSphere> innerBoundingSpheres;
        public Vector3 pos;
        public Vector3 innerShapePos;

        public BoundingShapes(Vector2 pos, BoundingBox outerBoundingBox) : 
            this(new Vector3(pos.X, pos.Y, 0), outerBoundingBox)
        {
        }

        public BoundingShapes(Vector3 pos, BoundingBox outerBoundingBox)
        {
            this.pos = pos;
            this.innerShapePos = pos;
            outerBoundingBox.Min += new Vector3(pos.X, pos.Y, 0);
            outerBoundingBox.Max += new Vector3(pos.X, pos.Y, 0);
            this.outerBoundingBox = outerBoundingBox;
            innerBoundingBoxes = new List<BoundingBox>();
            innerBoundingSpheres = new List<BoundingSphere>();
        }

        public void AddInnerBoundingBox(BoundingBox box)
        {
            box.Min += innerShapePos;
            box.Max += innerShapePos;
            innerBoundingBoxes.Add(box);
        }

        public void AddInnerBoundingSphere(BoundingSphere sphere)
        {
            sphere.Center += innerShapePos;
            innerBoundingSpheres.Add(sphere);
        }

        public bool CheckCollision(BoundingShapes other)
        {
            if (other == null)
                return false;

            //Check if outer boxes intersect, if they do look at inner
            if(outerBoundingBox.Intersects(other.outerBoundingBox)) 
            {
                UpdateInnerPositions();
                other.UpdateInnerPositions();
                if (innerBoundingBoxes.Count == 0 && innerBoundingSpheres.Count == 0
                    && other.innerBoundingBoxes.Count == 0 && other.innerBoundingSpheres.Count == 0) 
                {
                    //If no inner shapes return true
                    return true;
                }
                else if (other.innerBoundingBoxes.Count == 0 && other.innerBoundingSpheres.Count == 0)
                {
                    //Only this one has innerBoxes
                    //Check outer bounding box against other's inner boxes
                    foreach (BoundingBox box in innerBoundingBoxes)
                    {
                        if (box.Intersects(other.outerBoundingBox))
                            return true;
                    }

                    //Check outer bounding box against other's inner spheres
                    foreach (BoundingSphere sphere in innerBoundingSpheres)
                    {
                        if (sphere.Intersects(other.outerBoundingBox))
                            return true;
                    }
                }
                else if (innerBoundingBoxes.Count == 0 && innerBoundingSpheres.Count == 0)
                {
                    //Only other one has innerBoxes
                    //Check outer bounding box against other's inner boxes
                    foreach (BoundingBox otherBox in other.innerBoundingBoxes)
                    {
                        if (outerBoundingBox.Intersects(otherBox))
                            return true;
                    }

                    //Check outer bounding box against other's inner spheres
                    foreach (BoundingSphere otherSphere in other.innerBoundingSpheres)
                    {
                        if (outerBoundingBox.Intersects(otherSphere))
                            return true;
                    }
                }
                else
                {
                    //Both have inner bounding boxes
                    //Check inner bounding boxes
                    foreach (BoundingBox box in innerBoundingBoxes)
                    {
                        //Check inner bounding boxes against other's inner boxes
                        foreach (BoundingBox otherBox in other.innerBoundingBoxes)
                        {
                            if (box.Intersects(otherBox))
                                return true;
                        }

                        //Check inner bounding boxes against other's inner spheres
                        foreach (BoundingSphere otherSphere in other.innerBoundingSpheres)
                        {
                            if (box.Intersects(otherSphere))
                                return true;
                        }
                    }

                    //Check inner spheres
                    foreach (BoundingSphere sphere in innerBoundingSpheres)
                    {
                        //Check inner spheres against other's inner boxes
                        foreach (BoundingBox otherBox in other.innerBoundingBoxes)
                        {
                            if (sphere.Intersects(otherBox))
                                return true;
                        }

                        //Check inner spheres against other's inner spheres
                        foreach (BoundingSphere otherSphere in other.innerBoundingSpheres)
                        {
                            if (sphere.Intersects(otherSphere))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        //Only Check inner shape to save processing time
        //Hitmap doesnt need to be as insanely accurate
        public Color CheckHitMapCollision(Background background)
        {
            int yDiff = (int) (outerBoundingBox.Max.Y - outerBoundingBox.Min.Y);
            Color[,] hitmap = background.hitmap;
            if (hitmap != null)
            {
                for (int x = (int)outerBoundingBox.Min.X; x < outerBoundingBox.Max.X; x++)
                {
                    //Start at halfway down the y bounding box for accurate looking hitmap collision
                    for (int y = (int)outerBoundingBox.Min.Y + yDiff / 2; y < outerBoundingBox.Max.Y; y++)
                    {
                        if (x >= 0 && x < background.size.X && x >= 0 && y < background.size.Y)
                        {
                            if (!hitmap[x, y].Equals(Color.Transparent))
                                return hitmap[x, y];
                        }
                    }
                }
            }
            return Color.Transparent;
        }

        public bool PointInShapes(Vector3 point)
        {
            if (withinBox(outerBoundingBox, point))
            {
                return true;
            }

            return false;
        }

        public bool withinBox(BoundingBox box, Vector3 point)
        {
            return box.Min.X <= point.X && box.Min.Y <= point.Y
                && box.Max.X >= point.X && box.Max.Y >= point.Y;
        }

        public void UpdatePosition(Vector2 nextPos)
        {
            Vector3 nPos = new Vector3(nextPos.X, nextPos.Y, 0);
            Vector3 velocity = nPos - pos;
            this.pos.X += velocity.X;
            this.pos.Y += velocity.Y;
            outerBoundingBox.Min += new Vector3(velocity.X, velocity.Y, 0);
            outerBoundingBox.Max += new Vector3(velocity.X, velocity.Y, 0);
        }

        public void UpdateInnerPositions()
        {
            Vector3 change = pos - innerShapePos;
            innerShapePos = pos;
            List<BoundingBox> boxes = new List<BoundingBox>();
            for (int i = 0; i < innerBoundingBoxes.Count; i++)
            {
                boxes.Add(new BoundingBox(innerBoundingBoxes[i].Min + change, innerBoundingBoxes[i].Max + change));
            }
            innerBoundingBoxes = boxes;

            List<BoundingSphere> spheres = new List<BoundingSphere>();
            for (int i = 0; i < innerBoundingSpheres.Count; i++)
            {
                spheres.Add(new BoundingSphere(innerBoundingSpheres[i].Center + change, innerBoundingSpheres[i].Radius));
            }
            innerBoundingSpheres = spheres;
        }
    }
}
