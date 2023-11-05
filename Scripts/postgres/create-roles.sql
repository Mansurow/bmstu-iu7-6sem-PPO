-- Роль: Администратора

CREATE ROLE portal_admin WITH
    SUPERUSER
    CREATEDB
    CREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'PaS$woRdAdm1N';

-- права доступа

GRANT ALL PRIVILEGES 
    ON ALL TABLES IN SCHEMA public 
    TO portal_admin;
    
-- Роль: Пользователь (Авторизированный пользователь)

CREATE ROLE portal_user WITH
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'PaS$woRdUser';

-- права доступа

GRANT SELECT
    ON ALL TABLES IN SCHEMA public 
    TO portal_user;
    
GRANT INSERT
    ON public.users, 
       public.bookings, 
       public.feedbacks
    TO portal_user; 
    
GRANT DELETE
    ON public.bookings, 
       public.feedbacks
    TO portal_user;

GRANT UPDATE
    ON public.bookings,
       public.feedbacks
    TO portal_user; 
   
-- Роль: Гость (Неавторизированный пользователь) 

CREATE ROLE portal_guest WITH
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'PaS$woRdGuest';
    
-- права доступа

GRANT SELECT
    ON public.zones,
       public.inventories,
       public.feedbacks,
       public.packages,
       public.zone_packages,
       public.dishes,
       public.package_dishes
    TO portal_guest;

GRANT INSERT
    ON public.users
    TO portal_guest;
    
-- Удалить права доступа

-- REVOKE SELECT 
--     ON public."Users" 
--     TO portal_user;

-- Удалить роль

-- DROP ROLE guest;    