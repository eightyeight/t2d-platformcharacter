function PlatformCharacter::create(%this) {
   if(!isObject(PlatformControls)) {
      new ActionMap(PlatformControls);
      PlatformControls.bindObj(keyboard, a, "left", %this);
      PlatformControls.bindObj(keyboard, d, "right", %this);
      PlatformControls.bindObj(keyboard, w, "jump", %this);
      PlatformControls.push();
   }
}

function PlatformCharacter::destroy(%this) {
   if(isObject(PlatformControls)) {
      PlatformControls.pop();
      PlatformControls.delete();
   }
}

$PlatformCharacter::DefaultMoveSpeed = 50;
$PlatformCharacter::DefaultJumpSpeed = 5;
$PlatformCharacter::DefaultFriction = 1.1;

function PlatformCharacter::spawn(%input) {
   // Appearance
   %p = new Sprite() { class = "PlatformCharacterSprite"; };
   %p.setSize("1 2");

   // Collision/physics
   %p.setBodyType(dynamic);
   %p.FixedAngle = true;
   %p.setDefaultFriction($PlatformCharacter::DefaultFriction);
   %p.groundCollisionShape = %p.createPolygonBoxCollisionShape(1, 2);

   // Movement
   %p.moveX = 0;
   %p.setUpdateCallback(true);

   // Character properties
   %p.moveSpeed = $PlatformCharacter::DefaultMoveSpeed;
   %p.jumpSpeed = $PlatformCharacter::DefaultJumpSpeed;

   // Character properties
   %p.moveSpeed = $PlatformCharacter::DefaultMoveSpeed;
   %p.jumpSpeed = $PlatformCharacter::DefaultJumpSpeed;

   // Control
   switch$(%input) {
      case "primary":
         PlatformCharacter.Primary = %p;
      case "secondary":
         PlatformCharacter.Secondary = %p;
   }

   return %p;
}

function PlatformCharacter::left(%this, %val) {
   if(isObject(PlatformCharacter.Primary)) {
      %p = PlatformCharacter.Primary;
      %p.moveX -= %p.moveSpeed * (%val ? 1 : -1);
      %p.updateMovement();
   }
}

function PlatformCharacter::right(%this, %val) {
   if(isObject(PlatformCharacter.Primary)) {
      %p = PlatformCharacter.Primary;
      %p.moveX += %p.moveSpeed * (%val ? 1 : -1);
      %p.updateMovement();
   }
}

function PlatformCharacter::jump(%this, %val) {
   if(%val && isObject(PlatformCharacter.Primary)) {
      %p = PlatformCharacter.Primary;
      %p.setLinearVelocityY(%p.jumpSpeed);
   }
}

function PlatformCharacterSprite::updateMovement(%this) {
   %friction = 0.2;
   if(%this.moveX != 0) {
      %friction = 0;
   }
   %this.setCollisionShapeFriction(%this.groundCollisionShape, %friction);
}

function PlatformCharacterSprite::onUpdate(%this) {
   // Update movement force
   if(%this.moveX != 0) {
      %velX = %this.getLinearVelocityX();
      %force = %this.moveX / (mAbs(%velX) + 1) SPC 0;
      %this.applyForce(%force, %this.getPosition());
   }
}
