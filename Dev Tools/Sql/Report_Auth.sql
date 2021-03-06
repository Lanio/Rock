/* Group/Role Security on EntityType.  Does not include data for specific EntityIds */
SELECT  
      et.Name as [EntityType.Name]
      ,[Action]
      ,case [AllowOrDeny]
      when 'A' then 'Allow'
      when 'D' then 'Deny'
      else [AllowOrDeny] 
      end [Allow]
      ,case [SpecialRole]
      when 1 then '[All Users]'
      when 2 then '[All Authenticated Users]'
      when 3 then '[All Un-authenticated Users]'
      else g.Name
      end [Group/Role]
      ,a.[Order]
  FROM [Auth] a
  left outer join EntityType et on et.Id = a.EntityTypeId
  left outer join [Group] g on g.Id = a.GroupId
  where a.EntityId = 0
  order by et.Name, a.[Order]

/* Security on Rest Actions */
SELECT  
      ra.ApiId as [Rest Action]
      ,[Action]
      ,case [AllowOrDeny]
      when 'A' then 'Allow'
      when 'D' then 'Deny'
      else [AllowOrDeny] 
      end [Allow]
      ,case [SpecialRole]
      when 1 then '[All Users]'
      when 2 then '[All Authenticated Users]'
      when 3 then '[All Un-authenticated Users]'
      else g.Name
      end [Group/Role]
      ,a.[Order]
  FROM [Auth] a
  left outer join EntityType et on et.Id = a.EntityTypeId
  left outer join [Group] g on g.Id = a.GroupId
  join [RestAction] [ra] on a.EntityId = ra.Id
  where et.Name like 'Rock.Model.RestAction'
  order by ra.ApiId, a.[Order]

/* Security on Rest Controllers */
SELECT  
      rc.Name as [Rest Controller]
      ,[Action]
      ,case [AllowOrDeny]
      when 'A' then 'Allow'
      when 'D' then 'Deny'
      else [AllowOrDeny] 
      end [Allow]
      ,case [SpecialRole]
      when 1 then '[All Users]'
      when 2 then '[All Authenticated Users]'
      when 3 then '[All Un-authenticated Users]'
      else g.Name
      end [Group/Role]
      ,a.[Order]
  FROM [Auth] a
  left outer join EntityType et on et.Id = a.EntityTypeId
  left outer join [Group] g on g.Id = a.GroupId
  join [RestController] [rc] on a.EntityId = rc.Id
  where et.Name like 'Rock.Model.RestController'
  order by substring(rc.Name, 1, len(rc.Name) - 1), a.[Order]
