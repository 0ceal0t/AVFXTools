# AVFXTools
 
Here be dragons. Someone more knowledgable than me please help -_-

## TODO

### UI Stuff
- [x] UI and Graphics AVFX need to independent objects (this will allow adding/removing particles without breaking the graphics)
- [x] Add update button (will sync them: AVFXBase -> AVFXNode -> AVFXBase)
- [x] Choose local avfx file
- [x] Choose local mdl file
- [x] Base parameters
- [x] Particles
- [x] Emitters
- [x] Timelines
- [x] Binders
- [ ] Effectors
- [ ] Schedulers
- [x] Textures
- [x] Models
- [x] Maybe switch from `DragInt/Float` to `InputInt/Float`
- [x] Separate popups for model / avfx
- [ ] Option to remove model
- [ ] Better theme
- [ ] Choose game location + save settings
### General Graphics
- [ ] ⚠️ random (what do the random types mean)
- [ ] curves
- [ ] velocity
- [ ] gravity / velocity / position / air resistance
- [ ] Make sure bad bindings can be handled
- [ ] Clean up console log
- [ ] Check transformation order. In C#, it's `T * R * S` (probably)
### Particle Graphics
- [ ] ⚠️ so turns out powder particles don't need to have simple animations turned on. oops. rework all of that
- [x] ⚠️ draw mode (screen, reverse, multiply)
- [x] culling type
- [x] is depth test
- [x] is depth write
- [ ] ⚠️ soft particle
- [ ] tone map
- [ ] ⚠️ how colors / color scale interact
- [ ] brightness
- [ ] loop start / end
- [ ] ⚠️ fresnel
- [ ] texture normal
- [ ] texture reflection
- [ ] texture palette
- [ ] line particles
- [ ] windmill particles
- [ ] laser particles
- [ ] polygon particles
- [ ] polyline particles
- [ ] lightmodel particle (what's the difference?)
- [ ] morphmodel particle
- [ ] decal particle
- [ ] disc particle
- [ ] collision
- [ ] rotation base
- [ ] depth clip
- [ ] ⚠️ fix powder particles (gravity, etc)
- [ ] double-check quads
### Emitter Graphics
- [ ] CreateTime / CreateCount: what do those mean? They already exist elsewhere, why are they in the emitter sub-items?
- [ ] Create Probability
- [ ] Parent Influence Coords / Color
- [ ] Influence Coords
- [ ] Influence Coord Binder Position
- [ ] Inherit Parent Life
- [ ] Override Life
- [ ] Parameter Link
- [ ] Start frame
- [ ] Injection angle
- [ ] generate delay
- [ ] generate delay by one
- [ ] local direction
- [ ] any direction
- [ ] ⚠️ cylinder emitter
- [ ] ⚠️ sphere emitter
- [ ] cone emitter
- [ ] model emitter
- [ ] How does emitter movement interact with particle movement? are they bound?
- [ ] injection axis
- [ ] sound?
### Effector Graphics
- [ ] wut.
- [ ] pointlight
- [ ] directional light
- [ ] radial blur
- [ ] black hole
- [ ] camera quake
### Timeline Graphics
- [ ] How are emitters with a lifespan created?
- [ ] What does this even mean?
- [ ] clip
- [ ] start / end time
- [ ] parent influence
### Scheduler Graphics
- [ ] wut.
- [ ] trigger kicks
### Binder Graphics
- [ ] Bone mapping is probably wrong
- [ ] Origin
- [ ] Fitground
- [ ] Damaged circle
- [ ] By name
- [ ] linear
- [ ] spline
- [ ] camera
- [ ] bind to character / target

```
TODO:
scheduler
timeline clip
timeline start/end time
timeline parent influence
- scale / rotation / position
- inherit velocity
- life
- fit ground
injection angle
generate delay
binder rotation type
- standard
- billboard
- billboard axis y
- look at camera
- camera billboard axis y
binder ring
binder props
- scale, vfx scale
emitter looping
emitter effector
emitter anydirection
emitter types
rotation directions
- x,y,z
- move direction
- billboard axis y
- screen billboard
- move direction billboard
random types
- first +/-
- first +
- first -
- always +/-
- always +
- always -
generate method
- random to vertex
- order to vertex
- random on vertex
- random to vertex
- random to vertex without singular point
- order to vertex without singular point
- random on vertex without singular point
- rnaomd to vertex without singular point
color brightness
fix curves
lambert / half lambert
blending modes
powder particles
culling type
soft particle
tone map
fog
collision
clip
revised values
```