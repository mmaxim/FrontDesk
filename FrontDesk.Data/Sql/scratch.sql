USE FrontDesk

select g.*, a.description
from Groups g
	JOIN Assignments a ON a.ID = g.asstID

select gm.*, u.userName, g.groupName 
from GroupMembers gm 
	JOIN Users u ON gm.userID = u.principalID
	JOIN Groups g ON gm.groupID = g.principalID

select gi.*, tor.userName AS invitor, tee.userName AS invitee, g.groupName
from GroupInvitations gi
	JOIN Users tor ON gi.invitorID = tor.principalID
	JOIN Users tee ON gi.inviteeID = tee.principalID
	JOIN Groups g ON gi.groupID = g.principalID