
INSERT INTO sch_pyme.categoria
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('Bebidas Destiladas', now(), null, '', 'ADMINISTRADOR', '', 1);
INSERT INTO sch_pyme.categoria
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('Bebidas Fermentadas', now(), null, '', 'ADMINISTRADOR', '', 1);
INSERT INTO sch_pyme.categoria
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('Cervezas', now(), null, '', 'ADMINISTRADOR', '', 1);
INSERT INTO sch_pyme.categoria
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('Agua Ardiente', now(), null, '', 'ADMINISTRADOR', '', 1);

-- Insert para roles del sistema
-- estos son de forma manula ya que el sistema
-- esta dividido solo para estos roles
-- ADMINISTRADOR, PROVEEDOR, COMERCIANTE
-- CLIENTES no se tomara encuenta ya que 
-- todos los usuarios pueden ser clientes 
-- aqui se creara una tabla solo para clientes
INSERT INTO sch_pyme.rol
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('ADMINISTRADOR', now(), null, '', 'ADMINISTRADOR', null, 1);
INSERT INTO sch_pyme.rol
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('PROVEEDOR', now(), null, '', 'ADMINISTRADOR', null, 1);
INSERT INTO sch_pyme.rol
(nombre, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('COMERCIANTE', now(), null, '', 'ADMINISTRADOR', null, 1);


-- se crea el usuario administrador todo esto se debe hacer por base
-- la clave para todos los usuarios es encryptada
INSERT INTO sch_pyme.usuario
(nombres, apellidos, password, identificacion, telefono, Direccion, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES('administrador', 'administrador', 'AQAAAAIAAYagAAAAEEnYJ/ecQ6iH9+YIe3ltmXmHrnrZBXcX5NO6IFhevEMMTopd20Kc4tlCXmylgnOZcQ==', 'administrador', '', '', now(), null, '', 'ADMINISTRADOR', '', 1);


-- se crea el usuario rol para el administrador
INSERT INTO sch_pyme.usuariorol
(id_usuario, id_rol, created_at, updated_at, observacion, usuario_creacion, usuario_modificacion, activo)
VALUES((select id_usuario from usuario u where u.identificacion = 'administrador'), (select id_rol from rol r where r.nombre = 'ADMINISTRADOR'), now(), null, '', 'ADMINISTRADOR', '', 1);
