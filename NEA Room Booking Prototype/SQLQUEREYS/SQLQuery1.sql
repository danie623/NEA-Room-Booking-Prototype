SELECT * FROM TagAssign;
SELECT * FROM Tags;
SELECT * FROM Rooms;

SELECT *
FROM Rooms r, Tags t, TagAssign a
WHERE r.RoomID = a.RoomID AND a.TagID = t.TagID AND t.Tag = 'Aircon / Ceiling fans';